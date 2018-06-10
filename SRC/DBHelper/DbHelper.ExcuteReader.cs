using System;
using System.Data;

namespace DBHelper
{
    public partial class DbHelper
    {
        public IDataReader ExecuteReader(string cmdText)
        {
            return ExecuteReader(cmdText, CommandType.Text, CmdTimeOut, null);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType)
        {
            return ExecuteReader(cmdText, cmdType, CmdTimeOut, null);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType, params IDataParameter[] parameters)
        {
            return ExecuteReader(cmdText, cmdType, CmdTimeOut, parameters);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType, int cmdTimeout, params IDataParameter[] parameters)
        {
            return this.Excute(cmdText, cmdType, cmdTimeout, parameters, false, nameof(ExecuteReader),cmd => (cmd.Transaction == null ? cmd.ExecuteReader(CommandBehavior.CloseConnection) : cmd.ExecuteReader()));
        }
    }
}