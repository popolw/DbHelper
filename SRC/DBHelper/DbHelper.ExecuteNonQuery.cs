using System.Data;
using System.Data.Common;

namespace DBHelper
{
    public partial class DbHelper
    {
        public int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, CmdTimeOut, null, null);
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType)
        {
            return ExecuteNonQuery(cmdText, cmdType, CmdTimeOut, null, null);
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType, params IDataParameter[] parameters)
        {
            return ExecuteNonQuery(cmdText, cmdType, CmdTimeOut, null, parameters);
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType, DbTransactionScope trans, params IDataParameter[] parameters)
        {
            return ExecuteNonQuery(cmdText, cmdType, CmdTimeOut, trans, parameters);
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType, int cmdTimeout, DbTransactionScope trans, params IDataParameter[] parameters)
        {
            return this.Excute(cmdText, cmdType, cmdTimeout, trans, parameters, true, cmd => cmd.ExecuteNonQuery());
        }
    }

}