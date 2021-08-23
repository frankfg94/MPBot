using BT.Database.DbTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Database
{
    public static class Data
    {
        public static Dictionary<Comparator, string> operators { get; set; }
        public static List<SqlFunction> Functions { get; set; }
        public static Dictionary<LogicalOperator, string> logicalOperators { get; set; }

        public static void Init()
        {
            operators = new Dictionary<Comparator, string>();
            operators.Add(Comparator.EQUAL, " = ");
            operators.Add(Comparator.GREATER, " > ");
            operators.Add(Comparator.GREATER_OR_EQUAL, " >= ");
            operators.Add(Comparator.SMALLER, " < ");
            operators.Add(Comparator.SMALLER_OR_EQUAL, " <=");
            operators.Add(Comparator.UNEQUAL, " != ");
            operators.Add(Comparator.LIKE, " LIKE ");
            operators.Add(Comparator.BEGINLIKE, " LIKE ");
            operators.Add(Comparator.ENDLIKE, " LIKE ");
            operators.Add(Comparator.IS_NOT, " IS NOT ");
            operators.Add(Comparator.IS, " IS ");
            operators.Add(Comparator.BETWEEN, " BETWEEN ");

            logicalOperators = new Dictionary<LogicalOperator, string>();
            logicalOperators.Add(LogicalOperator.AND, "AND");
            logicalOperators.Add(LogicalOperator.OR, "OR");
            logicalOperators.Add(LogicalOperator.NONE, "");
        }
    }
}
