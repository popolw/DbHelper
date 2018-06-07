using System;
using System.Data;

namespace DBHelper
{
    public class DbTransactionScope : IDisposable
    {
        private DbTransactionScope _root;
        public bool Complete{get;private set;}
        public DbProvider Provider { get; private set; }
        internal IDbConnection DbConnection{get;private set;}
        internal IDbTransaction DbTransaction {get;private set; }
        public DbTransactionScope(DbProvider provider)
        {
             this._root = this;
             this.Provider=provider;
             this.DbConnection = DbHelper.CreateConnection(provider);
             this.DbTransaction = DbConnection.BeginTransaction(); 
        }

        public DbTransactionScope(DbTransactionScope scope)
        {
            this._root = scope._root;
            this.Provider=scope.Provider;
            this.DbTransaction=scope.DbTransaction;
            this.DbConnection=this.DbTransaction.Connection;
        }


        public void Commit()
        {
            if(this !=_root) return;
            this.DbTransaction.Commit();
            this.Complete=true;
            this.Dispose();
        }

        public void Dispose()
        {
            if(this!=_root) return;
            if(!this.Complete)
            {
               this.DbTransaction.Rollback();                 
            }
            this.DbConnection.Close();
            this.DbTransaction.Dispose();
        }

    }
}