﻿using System;
using System.Data;
using System.Data.Common;

namespace DBHelper
{
    public partial class DbHelper
    {
        public static int CmdTimeOut = 30;

        internal DbProvider Provider { get; private set; }

        public DbHelper(DbProvider provider)
        {
            this.Provider = provider;
        }

        private T Excute<T>(string cmdText, CommandType cmdType, int cmdTimeout, DbTransactionScope trans, IDataParameter[] parameters, bool close, Func<IDbCommand, T> func)
        {
            IDbCommand cmd = null;
            try
            {
                cmd = this.CreateCommand(cmdText, cmdType, cmdTimeout, trans, parameters);
                return func(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (trans == null && close)
                {
                    if (cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                    {
                        cmd.Connection.Close();
                        cmd.Dispose();
                    }
                }
            }

        }

        internal static IDbConnection CreateConnection(DbProvider provider)
        {
            var connection = provider.DbFactory.CreateConnection();
            connection.ConnectionString = provider.ConnectionString;
            connection.Open();
            return connection;
        }

        private IDbCommand CreateCommand(string cmdText, CommandType cmdType, int cmdTimeout, DbTransactionScope transaction, params IDataParameter[] parameters)
        {
            var cmd = transaction != null ? transaction.DbConnection.CreateCommand() : CreateConnection(this.Provider).CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            cmd.Transaction = transaction?.DbTransaction;
            cmd.CommandTimeout = cmdTimeout;
            if (parameters != null)
            {
                cmd.Parameters.Clear();
                foreach (var parameter in parameters)
                {
                    var p = parameter;
                    if (p.Value == null)
                    {
                        //dbnull fix
                        p.Value = DBNull.Value;
                    }
                    if (parameter is DynamicParameter)
                    {
                       p = ((DynamicParameter)parameter).GetDataParameter(cmd);     
                    }
                    cmd.Parameters.Add(p);
                }
            }
            return cmd;
        }
    }
}
