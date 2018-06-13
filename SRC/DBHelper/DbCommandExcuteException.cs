using System;
using System.Data;
using System.Reflection;

namespace DBHelper
{
    public sealed class DbCommandExcuteException : Exception
    {
        public string TransactionId { get; private set; }
        public string CmdText{ get; private set; }
        public CommandType CmdType { get; private set; }

        public IDataParameter[] Parameters { get; private set; }

        public string MethodName{get;private set;}
        public DbCommandExcuteException(string cmdText, CommandType cmdType, IDataParameter[] parameters,string methodName,string transid,Exception innerException):base(innerException.Message,innerException)
        {
            this.CmdText = cmdText;
            this.CmdType = cmdType;
            this.Parameters = parameters;
            this.MethodName=methodName;
            this.TransactionId=transid;
        }
    }
}