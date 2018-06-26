using System;
using System.Data;
using System.Data.Common;

namespace DBHelper
{
    public partial class DbHelper
    {
        public int CmdTimeOut = 30;

        internal DbProvider Provider { get; private set; }

        public DbHelper(DbProvider provider)
        {
            this.Provider = provider;
        }

        public DbHelper(string name, string connectionString, string providerName,Func<string,DbProviderFactory> func)
        :this(new DbProvider(name,connectionString,providerName,func))
        {

        }

        private T Excute<T>(string cmdText, CommandType cmdType, int cmdTimeout,IDataParameter[] parameters, bool close,string methodName, Func<IDbCommand, T> func)
        {
            var current = AdoNetTransactionWrap.Current;
            IDbCommand cmd = null;
            try
            {
               
                cmd = this.CreateCommand(cmdText, cmdType, cmdTimeout, current, parameters);
                return func(cmd);
            }
            catch (Exception ex)
            {
                throw new DbCommandExcuteException(cmdText, cmdType, parameters,methodName, current?.Id, ex);
            }
            finally
            {
                if (current == null && close)
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

        private IDbCommand CreateCommand(string cmdText, CommandType cmdType, int cmdTimeout, AdoNetTransaction transaction, params IDataParameter[] parameters)
        {
            var cmd = transaction != null ? transaction.Connection.CreateCommand() : CreateConnection(this.Provider).CreateCommand();
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            cmd.Transaction = transaction?.Current;
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



        public IDataParameter CreateParameter(string parameterName,object value,DbType dbType)
        {
            return this.CreateParameter(parameterName,value,0,ParameterDirection.Input,dbType);
        }

        public IDataParameter CreateParameter(string parameterName,object value,ParameterDirection direction,DbType dbType)
        {
            return this.CreateParameter(parameterName,value,0,direction,dbType);
        }


        public IDataParameter CreateParameter(string parameterName,object value,int size,ParameterDirection direction,DbType dbType)
        {
            return this.CreateParameter(parameterName,value,size,dbType,direction,0,0);
        }

        /// <summary>
        /// 创建一个数据库参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="size">参数长度</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="direction">参数方向</param>
        /// <param name="precision">最大位数</param>
        /// <param name="scale">小数位数</param>
        /// <returns>IDataParameter</returns>
        public IDataParameter CreateParameter(string parameterName,object value,int size,DbType dbType,ParameterDirection direction,byte precision,byte scale)
        {
            var parameter = this.Provider.DbFactory.CreateParameter();
            parameter.ParameterName=parameterName;
            parameter.Value=value;
            parameter.DbType=dbType;
            parameter.Scale=scale;
            parameter.Precision=precision;
            return parameter;
        }

    }
}
