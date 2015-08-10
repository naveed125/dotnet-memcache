using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMemCache
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var watch = Stopwatch.StartNew();
            var customerList = Customers.get("Facebook");
            watch.Stop();

            foreach (var customer in customerList)
            {
                Console.WriteLine(
                    "Id:{0}\nName:{1}\nEmail:{2}\nCompany:{3}\nPhone:{4}\n----\n",
                    customer.Id, customer.Name, customer.Email, customer.Company, customer.Phone);
            }

            if (Customers.MemCacheHit)
            {
                Console.WriteLine("MemCache was hit.");
            }
            else
            {
                Console.WriteLine("MemCache was NOT hit.");
            }
            Console.WriteLine(String.Format("Took {0} miliseconds", watch.ElapsedMilliseconds));

            Console.ReadLine();
        }

    }
}
