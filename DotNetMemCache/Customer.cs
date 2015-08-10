using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMemCache
{
    /// <summary>
    /// Model class - important: must be marked Serializable
    /// </summary>
    [Serializable]
    class Customer
    {
        public int Id;
        public string Name;
        public string Email;
        public string Company;
        public string Phone;

    }

    /// <summary>
    /// Collection class
    /// </summary>
    class Customers {

        public static bool MemCacheHit = false;

        private static MemCacheHelper _mch = null;
        private static MemCacheHelper mch
        {
            get
            {
                if (_mch == null)
                {
                    _mch = new MemCacheHelper();
                }
                return _mch;
            }

        }

        /// <summary>
        /// public get method tried memcache then db
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IEnumerable<Customer> get(string Company)
        {
            // try in memcache first
            MemCacheHit = false;
            var ret = getFromMC(Company);
            if (ret != null)
            {
                MemCacheHit = true;
                return ret;
            }

            // load from db
            ret = getFromDB(Company);

            // store in memcache for next time
            MemCacheHit = false;
            var key = mch.key("customer", new Dictionary<string, string>() { { "company", Company } });
            mch.set(key, ret);

            return ret;
        }

        /// <summary>
        /// Get customer list from memcache
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        private static IEnumerable<Customer> getFromMC(string Company)
        {
            var key = mch.key("customer", new Dictionary<string, string>() { { "company", Company } });
            return (IEnumerable<Customer>)mch.get(key);
        }

        /// <summary>
        /// Get customer list from database
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        private static IEnumerable<Customer> getFromDB(string Company)
        {
            string queryString = "SELECT Id, Name, Email, Company, Phone from dbo.Customers WHERE Company = @Company ORDER BY Id DESC;";
            string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=c:\projects\DotNetMemCache\DotNetMemCache\Database1.mdf;Integrated Security=True";
            using (SqlConnection connection =  new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // uncomment the following 2 lines if you like to generate more data
                    //var generator = new CustomerDataGenerator(connection);
                    //generator.generate(1000);

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@Company", Company);
                    SqlDataReader reader = command.ExecuteReader();
                    var customers = new List<Customer>();
                    while (reader.Read())
                    {
                        customers.Add(new Customer()
                        {
                            Id = (int)reader[0],
                            Name = (string)reader[1],
                            Email = (string)reader[2],
                            Company = (string)reader[3],
                            Phone = (string)reader[4],
                        });
                    }

                    reader.Close();
                    return customers;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return null;
        }

    }
}
