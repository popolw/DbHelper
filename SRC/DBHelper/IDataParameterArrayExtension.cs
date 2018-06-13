using System;
using System.Data;

namespace DBHelper
{
   public static class IDataParameterArrayExtension
    {
        internal static int IndexOf(this IDataParameter[] parameters, string parameterName)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (string.Equals(parameters[i].ParameterName, parameterName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        public static IDataParameter GetParameter(this IDataParameter[] parameters, string parameterName)
        {
            if (parameters == null) return null;
            var index = parameters.IndexOf(parameterName);
            return index < 0 ? null : parameters[index];
        }
    }
}