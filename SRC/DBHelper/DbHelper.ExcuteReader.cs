using System;
using System.Data;

namespace DBHelper
{
    public partial class DbHelper
    {
        public IDataReader ExecuteReader(string cmdText)
        {
            return ExecuteReader(cmdText, CommandType.Text, CmdTimeOut, null, null);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType)
        {
            return ExecuteReader(cmdText, cmdType, CmdTimeOut, null, null);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType, params IDataParameter[] parameters)
        {
            return ExecuteReader(cmdText, cmdType, CmdTimeOut, null, parameters);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType, DbTransactionScope trans, params IDataParameter[] parameters)
        {
            return ExecuteReader(cmdText, cmdType, CmdTimeOut, trans, parameters);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType, int cmdTimeout, DbTransactionScope trans, params IDataParameter[] parameters)
        {
            return this.Excute(cmdText, cmdType, cmdTimeout, trans, parameters, false, cmd => (trans == null ? cmd.ExecuteReader(CommandBehavior.CloseConnection) : cmd.ExecuteReader()));
        }
    }
}