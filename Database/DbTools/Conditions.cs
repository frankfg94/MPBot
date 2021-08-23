using BT.Database.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Database.DbTools
{
    public class Conditions
    {
        public Column Key { get; set; }
        public Comparator Comparator { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
        public LogicalOperator LogicalOperator { get; set; }



        public Conditions(Column key, object value, object value2 = null, Comparator comparator = Comparator.EQUAL, LogicalOperator logicalOperator = LogicalOperator.NONE)
        {
            Key = key;
            Comparator = comparator;
            Value = value;
            LogicalOperator = logicalOperator;
        }

        public Conditions(string key, object value, object value2 = null, Comparator comparator = Comparator.EQUAL, LogicalOperator logicalOperator = LogicalOperator.NONE)
        {
            Key = new Column(name: key);
            Comparator = comparator;
            Value = value;
            LogicalOperator = logicalOperator;
        }

        public Conditions()
        {

        }

        public string GetSql()
        {
            string Sql = "";
            if (Value != null)
                Sql += " " + Key.GetSql(false) + Data.operators[Comparator] + "@p" + DbRequester.ParameterIndex + " ";
            /*else
                Sql += " " + Key.GetSql(false) + " " + Data.operators[Comparator] + " NULL ";*/
            switch (this.Comparator)
            {
                case Comparator.LIKE:
                    Value = "%" + Value + "%";
                    break;
                case Comparator.BEGINLIKE:
                    Value = Value + "%";
                    break;
                case Comparator.ENDLIKE:
                    Value = "%" + Value;
                    break;
                case Comparator.BETWEEN:
                    Sql += "AND " + "@p" + DbRequester.ParameterIndex + 1 + " ";
                    break;
                case Comparator.IS:
                    Sql += " " + Key.GetSql(false) + " " + Data.operators[Comparator] + " NULL ";
                    break;
                case Comparator.IS_NOT:
                    Sql += " " + Key.GetSql(false) + " " + Data.operators[Comparator] + " NULL ";
                    break;
            }
            return Sql;
        }

        public bool HasAggregates()
        {
            if (!Key.Functions.IsNullOrEmpty())
            {
                foreach (SqlFunction function in Key.Functions)
                {
                    if (function.Aggregate)
                        return true;
                }
            }
            return false;
        }

    }

    public enum Comparator
    {
        EQUAL,
        UNEQUAL,
        GREATER,
        GREATER_OR_EQUAL,
        SMALLER,
        SMALLER_OR_EQUAL,
        IS,
        IS_NOT,
        LIKE,
        BEGINLIKE,
        ENDLIKE,
        BETWEEN
    }

    public enum LogicalOperator
    {
        AND,
        OR,
        NONE
    }
}
