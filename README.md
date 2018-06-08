# DbHelper
using DBHelper;
using System;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db.data");
            var connection = $@"Data Source={path};Pooling=true;FailIfMissing=false";

            var helper = new DbHelper(new DbProvider("C1", connection, "System.Data.SQLite", _ => SQLiteFactory.Instance));
            var sb = new StringBuilder();
            using (var reader = helper.ExecuteReader("select * from users"))
            {
                while (reader.Read())
                {
                    var id = reader["id"].ToString();
                    var name = reader["name"].ToString();
                    sb.AppendLine($"id:{id},name:{name}");
                }
            }
            Console.WriteLine(sb.ToString());
            Console.Read();
        }
    }
}

