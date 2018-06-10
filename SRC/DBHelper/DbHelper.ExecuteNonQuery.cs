using System.Data;
using System.Data.Common;

namespace DBHelper
{
    public partial class DbHelper
    {
        public int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, CmdTimeOut, null);
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType)
        {
            return ExecuteNonQuery(cmdText, cmdType, CmdTimeOut, null);
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType, params IDataParameter[] parameters)
        {
            return ExecuteNonQuery(cmdText, cmdType, CmdTimeOut, parameters);
        }

        public int ExecuteNonQuery(string cmdText, CommandType cmdType, int cmdTimeout,params IDataParameter[] parameters)
        {
            return this.Excute(cmdText, cmdType, cmdTimeout, parameters, true,nameof(ExecuteNonQuery) ,cmd => cmd.ExecuteNonQuery());
        }
    }

}