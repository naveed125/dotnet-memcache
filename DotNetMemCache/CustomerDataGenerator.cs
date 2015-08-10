using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMemCache
{
    class CustomerDataGenerator
    {
        private SqlConnection _connection = null;
        public CustomerDataGenerator(SqlConnection connection)
        {
            _connection = connection;
        }

        public bool generate(int count)
        {
            if (_connection == null)
            {
                return false;
            }

            try
            {
                string queryString = "INSERT INTO dbo.Customers (Name, Email, Company, Phone) VALUES (@customerName, @customerEmail, @customerCompany, @customerPhone)";
                for (int i = 0; i < count; i++)
                {
                    SqlCommand command = new SqlCommand(queryString, _connection);
                    command.Parameters.AddWithValue("@customerName", String.Format("Customer{0}Name", i));
                    command.Parameters.AddWithValue("@customerEmail", String.Format("Customer{0}@Email", i));
                    command.Parameters.AddWithValue("@customerCompany", getRandomCompanyName());
                    command.Parameters.AddWithValue("@customerPhone", String.Format("Customer{0}Phone", i));
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private string getRandomCompanyName()
        {
            var companies = new string[] {
                "Microsoft",
                "Facebook",
                "Google",
                "Oracle",
                "Yahoo",
                "Twitter",
                "Amazon"
            };

            var random = new Random((int)DateTime.Now.Ticks);
            return companies[random.Next(0, companies.Count())];
        }
    }
}
