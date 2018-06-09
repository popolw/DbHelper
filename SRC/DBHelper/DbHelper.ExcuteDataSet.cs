using System.Data;
using System.Data.Common;

namespace DBHelper
{
    public partial class DbHelper
    {
        public DataSet Query(string cmdText)
        {
            return Query(cmdText, CommandType.Text, CmdTimeOut,null);
        }

        public DataSet Query(string cmdText, CommandType cmdType)
        {
            return Query(cmdText, cmdType, CmdTimeOut, null);
        }

        public DataSet Query(string cmdText, CommandType cmdType, params IDataParameter[] parameters)
        {
            return Query(cmdText, cmdType, CmdTimeOut, parameters);
        }

        public DataSet Query(string cmdText, CommandType cmdType, int cmdTimeout,params IDataParameter[] parameters)
        {
            return this.Excute(cmdText, cmdType, cmdTimeout, parameters, true, cmd =>
             {
                 var adapter = this.Provider.DbFactory.CreateDataAdapter();
                 var set = new DataSet();
                 adapter.SelectCommand = (DbCommand)cmd;
                 adapter.Fill(set);
                 return set;
             });
        }
    }
}