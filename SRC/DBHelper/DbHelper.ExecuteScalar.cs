using System;
using System.Data;

namespace DBHelper
{
    public partial class DbHelper
    {
        public object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(cmdText, CommandType.Text, CmdTimeOut, null, null);
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType)
        {
            return ExecuteScalar(cmdText, cmdType, CmdTimeOut, null, null);
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType, params IDataParameter[] parameters)
        {
            return ExecuteScalar(cmdText, cmdType, CmdTimeOut, null, parameters);
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType, DbTransactionScope trans, params IDataParameter[] parameters)
        {
            return ExecuteScalar(cmdText, cmdType, CmdTimeOut, trans, parameters);
        }

        public object ExecuteScalar(string cmdText, CommandType cmdType, int cmdTimeout, DbTransactionScope trans, params IDataParameter[] parameters)
        {
            return this.Excute(cmdText, cmdType, cmdTimeout, trans, parameters, true, cmd => cmd.ExecuteScalar());
        }
    }
}