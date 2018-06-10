# DbHelper

using DBHelper;<br/>
using System;<br/>
using System.Data.SQLite;<br/>
using System.IO;<br/>
using System.Text;<br/>


namespace Test 
{

    class Program	
	
    {
    
        static void Main(string[] args)
        {
        
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db.data");
            var connection = $@"Data Source={path};Pooling=true;FailIfMissing=false";
            var provider = new DbProvider("C1", connection, "System.Data.SQLite", _ => SQLiteFactory.Instance);
            var helper = new DbHelper(provider);
            var sb = new StringBuilder();
            using (DbTransactionScope scope = new DbTransactionScope(provider))
            {
                var sql = $"insert into users([name])values('{Guid.NewGuid().ToString()}')";
                var row = helper.ExecuteNonQuery(sql);

                //if i have another method which has a transaction 
                using (DbTransactionScope scope2 = new DbTransactionScope(provider))
                {
                    var sql2 = $"insert into users([name])values('哎呀呀{Guid.NewGuid().ToString()}')";
                    var row2 = helper.ExecuteNonQuery(sql2);
                    //in fact the transaction will not commit
                    scope2.Commit();
                }

                //will rell commit
                scope.Commit();
            }
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

