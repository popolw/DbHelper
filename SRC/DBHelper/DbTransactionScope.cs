using System;
using System.Data;
using System.Data.Common;

namespace DBHelper {
    internal class AdoNetTransaction : DbTransaction, IDbTransaction {
        private DbConnection _connection;
        private DbTransaction _transaction;
        public AdoNetTransaction (DbProvider provider) {
            this._connection = (DbConnection) DbHelper.CreateConnection (provider);
            this._transaction = this._connection.BeginTransaction ();
        }

        public new DbConnection Connection { get => this.DbConnection; }
        public override IsolationLevel IsolationLevel => this._transaction.IsolationLevel;

        protected override DbConnection DbConnection => this._connection;

        public override void Commit () {
            this._transaction.Commit ();
        }

        public override void Rollback () {
            this._transaction.Rollback ();
        }

        protected override void Dispose (bool disposing) {
            this._transaction.Dispose ();
        }
    }

    internal class AdoNetTransactionWrap {
        [ThreadStatic]
        private static AdoNetTransaction _current;
        public static AdoNetTransaction Current { get => _current; set => _current = value; }
    }

    public class DbTransactionScope : IDisposable {

        private bool _root;
        private AdoNetTransaction _transaction;
        public bool Completed { get; private set; }
        public DbTransactionScope (DbProvider provider) {
            this._transaction = AdoNetTransactionWrap.Current;
            if (this._transaction == null) {
                this._root = true;
                this._transaction = new AdoNetTransaction (provider);
                AdoNetTransactionWrap.Current = this._transaction;
            } else {
                this._transaction = AdoNetTransactionWrap.Current;
            }
        }

        public void Commit () {
            if (this.Completed || !this._root) return;
            this._transaction.Commit ();
            this.Completed = true;
        }

        public void Dispose () {

            if (this._root) {
                if (!this.Completed) {
                    this._transaction.Rollback ();
                }
                this._transaction.Dispose ();
                this._transaction = null;
                AdoNetTransactionWrap.Current = null;
            }

        }
    }
}