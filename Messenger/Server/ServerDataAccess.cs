using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using Server.Models;

namespace Server
{
    public class ServerDataAccess
    {
        
        private static string connectionString = "Data Source=ServerDataBase.db;Version=3;";
        public static void GetRows()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                IEnumerable<User> users = connection.Query<User>("SELECT * FROM User");

                connection.Close();

                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.id}, username: {user.username}, pass: {user.password}, key: {user.key}");
                }


                
            }
        }
    }
}
