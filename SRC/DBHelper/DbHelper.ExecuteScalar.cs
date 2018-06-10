using System;
using System.Data;

namespace DBHelper
{
    public partial class DbHelper
    {
        public object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(cmdText, CommandType.Text, CmdTimeOut, null);
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType)
        {
            return ExecuteScalar(cmdText, cmdType, CmdTimeOut,null);
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType, DbTransactionScope trans, params IDataParameter[] parameters)
        {
            return ExecuteScalar(cmdText, cmdType, CmdTimeOut, parameters);
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType, int cmdTimeout, params IDataParameter[] parameters)
        {
            return this.Excute(cmdText, cmdType, cmdTimeout, parameters, true,nameof(ExecuteScalar),cmd => cmd.ExecuteScalar());
        }
    }
}