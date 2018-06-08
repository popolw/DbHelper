using System;
using System.Data;

namespace DBHelper
{
    public class DbTransactionScope : IDisposable
    {
        private DbTransactionScope _root;

        /// <summary>
        /// 事务是否完成(始终是为根事务)
        /// </summary>
        /// <returns>bool</returns>
        public bool Completed{get=>this._root.Completed;private set=>this._root.Completed=value;}
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
            this.Completed=true;
        }

        public void Dispose()
        {
            if(this!=_root) return;
            if(!this.Completed)
            {
               this.DbTransaction.Rollback();                 
            }
            this.DbConnection.Close();
            this.DbTransaction.Dispose();
        }

    }
}