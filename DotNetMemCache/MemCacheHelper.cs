using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace DotNetMemCache
{
    class MemCacheHelper
    {
        const string MEMCACHE_HOST = "pub-memcache-14551.us-west-2-1.1.ec2.garantiadata.com";
        const int MEMCACHE_PORT = 14551;

        /// <summary>
        /// Instance of the EnyimMemcached client
        /// Installed using NuGet console using: Install-Package EnyimMemcached
        /// </summary>
        private MemcachedClient _mc = null;

        /// <summary>
        /// Contructor
        /// </summary>
        public MemCacheHelper()
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            config.Servers.Add(GetIPEndPointFromHostName(MEMCACHE_HOST, MEMCACHE_PORT));
            config.Protocol = MemcachedProtocol.Binary;
            config.Authentication.Type = typeof(PlainTextAuthenticator);
            config.Authentication.Parameters["userName"] = "dotnet";
            config.Authentication.Parameters["password"] = "memcache";
            config.Authentication.Parameters["zone"] = "";

            _mc = new MemcachedClient(config); 

        }

        /// <summary>
        /// Helper function to convert host to IPEndPoint needed by Enyim Memcache client
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private IPEndPoint GetIPEndPointFromHostName(string hostName, int port)
        {
            var addresses = System.Net.Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException("Could not find host");
            }

            return new IPEndPoint(addresses[0], port);
        }

        /// <summary>
        /// main entry point to get an object using a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object get(string key)
        {
            var ret = _mc.Get(key);
            return ret;
        }

        /// <summary>
        /// set in memcache with time to live in seconds
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        public bool set(string key, Object value, int ttl = 10)
        {
            var ret = _mc.Store(StoreMode.Set, key, value, new TimeSpan(0, 0, ttl));
            return ret;
        }

        /// <summary>
        /// helper function generate a uniq key
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string key(string prefix, Dictionary<string, string> parameters)
        {
            string ret = prefix + "?";
            foreach (var parameter in parameters)
            {
                ret += String.Format("{0}={1}", parameter.Key, parameter.Value);
            }

            return ret;
        }

    }
}
