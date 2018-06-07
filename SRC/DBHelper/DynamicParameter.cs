using System;
using System.Data;
using System.Data.Common;

namespace DBHelper
{
    /// <summary>
    /// 动态数据库参数
    /// </summary>
    public sealed class DynamicParameter : DbParameter,IDataParameter, IDbDataParameter
    {
        private IDbDataParameter _parameter;

        /// <summary>
        /// 默认无参构造函数
        /// </summary>
        public DynamicParameter()
        {
            SourceVersion = DataRowVersion.Current;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="patameterName">参数名称</param>
        /// <param name="value">参数值</param>
        public DynamicParameter(string patameterName, object value)
        {           
            this.ParameterName = patameterName;
            this.Value = value;
            SourceVersion = DataRowVersion.Current;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="patameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="size">长度</param>
        public DynamicParameter(string patameterName, object value,int size)
        {
            this.Size = size;
            this.Value = value;
            this.ParameterName = patameterName;
            SourceVersion = DataRowVersion.Current;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="patameterName">参数名称</param>
        /// <param name="value">值</param>
        /// <param name="size">长度</param>
        /// <param name="dbType">数据类型</param>
        public DynamicParameter(string patameterName, object value,int size,DbType dbType)
        {
            this.Value = value;
            this.Size = size;
            this.DbType = dbType;
            this.ParameterName = patameterName;
            this.SourceVersion = DataRowVersion.Current;
        }

        /// <summary>
        /// 长度
        /// </summary>
        public override int Size { get; set; }

        public override byte Precision { get; set; }


        public override  byte Scale { get; set; }


        public override DbType DbType { get; set; }

 

        /// <summary>
        /// 参数方向
        /// </summary>
        public override ParameterDirection Direction { get; set; }

        /// <summary>
        /// 是否是
        /// </summary>
        public override bool IsNullable { get; set; }

        public override string ParameterName { get; set; }

        public override string SourceColumn { get; set; }

        private object _value;
        public override object Value 
        {
            get 
            {
                //bugfix 输出参数 2015-04-03
                return (_parameter != null) ? _parameter.Value : _value;
            }
            set 
            {
                _value = value;
            }
        }


        public override DataRowVersion SourceVersion { get; set; }

        public override bool SourceColumnNullMapping { get; set; }
       

        public override void ResetDbType()
        {
            
        }

        internal IDataParameter GetDataParameter(IDbCommand cmd)
        {
            _parameter = cmd.CreateParameter();
            _parameter.ParameterName = ParameterName.StartsWith("@") ? ParameterName : "@" + ParameterName;
          
            if (_value is Enum)
                _parameter.Value = Convert.ToInt32(_value);
            else _parameter.Value = _value;
            _parameter.Size = this.Size;
            _parameter.Precision = this.Precision;
            _parameter.SourceVersion = SourceVersion;
            _parameter.SourceColumn = SourceColumn;
            _parameter.DbType = DbType;
            _parameter.Scale = this.Scale;
            _parameter.Direction = Direction;
            return _parameter;
        }

        public override string ToString()
        {
            return string.Format("Name：{0}  Value：{1}",this.ParameterName,this.Value);
        }
    }
}