using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.Database.DbTools
{
    public class SqlFunction
    {
        public ColumnFunction FunctionName { get; set; }
        public string Name { get; set; }
        public bool Aggregate { get; set; }
        public bool Argument { get; set; }
        public object[] Arguments { get; set; }

        public SqlFunction(ColumnFunction functionName = ColumnFunction.NONE, string name = null, bool aggregate = false, bool argument = false, object[] arguments = null)
        {
            if (name == null) Name = functionName.ToString();
            else Name = name;
            FunctionName = functionName;
            Argument = argument;
            Arguments = arguments;
            Aggregate = aggregate;
        }

    }

    public enum ColumnFunction
    {
        NONE,
        SUBSTRING,
        COUNT,
        SUM,
        AVG,
        MIN,
        MAX,
        DISTINCT,
        YEAR,
        CAST_AS_INT
    }
}
