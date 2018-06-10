using System;
using System.Data;
using System.Data.Common;

namespace DBHelper {
    internal class AdoNetTransaction:DbTransaction  {

        private DbTransaction _transaction;

        public IDbTransaction Current { get => this._transaction; }

        public AdoNetTransaction (DbProvider provider) {
            this.Id = Guid.NewGuid().ToString("N");
            this.Connection = (DbConnection) DbHelper.CreateConnection (provider);
            this._transaction = this.Connection.BeginTransaction ();
        }

        public string Id { get;private set; }

        public new  DbConnection Connection { get;private set; }
        public  override IsolationLevel IsolationLevel => this._transaction.IsolationLevel;

        protected override DbConnection DbConnection => this.Connection;

        public override  void Commit () {
            this._transaction.Commit ();
        }

        public override  void Rollback () {
            this._transaction.Rollback ();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this._transaction.Dispose();
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