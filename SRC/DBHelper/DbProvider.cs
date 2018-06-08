using System;
using System.Data.Common;

namespace DBHelper
{
    public class DbProvider
    {
        internal DbProviderFactory DbFactory { get; private set; }
        public string Name { get; private set; }


        public string ConnectionString { get; private set; }


        public string ProviderName { get; private set; }

        public DbProvider(string name, string connectionString, string providerName,Func<DbProviderFactory> func)
        {
            if(func==null) throw new ArgumentNullException(nameof(func));
            this.Name = name;
            this.ConnectionString = connectionString;
            this.ProviderName = providerName;
        }

        public override string ToString()
        {
            return string.Format("Name：[{0}] ConnectionString：[{1}] Provider：[{2}]", this.Name, this.ConnectionString, this.ProviderName);
        }
    }
}