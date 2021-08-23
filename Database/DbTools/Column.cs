using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Database.DbTools
{
    public class Column
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Table { get; set; }
        public SqlFunction[] Functions { get; set; }

        public Column(SqlFunction[] functions = null, string name = "*", string alias = "", string table = "")
        {
            Name = name;
            if (alias != "")
                Alias = alias;
            else if (functions != null)
            {
                Alias = "";
                foreach (SqlFunction function in functions)
                    Alias += function.Name + " ";
                Alias += name;
            }
            else
                Alias = name;
            Table = table;
            Functions = functions;
            if (functions != null)
            {
                double l = Functions.Length / 2;
                int n = Functions.Length;
                for (int i = 0; i < l; i++)
                {
                    SqlFunction temp = Functions[n - i - 1];
                    Functions[n - i - 1] = Functions[i];
                    Functions[i] = temp;
                }
            }
            //SetSql(name, table, functions);
        }

        public string GetSql(bool alias = true)
        {
            string Sql = "";
            if (Name.Equals("*"))
                Sql = Name;
            else if (string.IsNullOrEmpty(Table))
                Sql = "[" + Name + "]";
            else
                Sql = Table + "." + Name;
            if (Functions != null)
            {
                int aggNumber = 0;
                foreach (var func in Functions)
                {
                    if (func.Aggregate)
                        aggNumber++;
                }
                if (aggNumber <= 1)
                {
                    foreach (var f in Functions)
                    {
                        switch (f.FunctionName)
                        {
                            case ColumnFunction.AVG:
                                Sql = "AVG(CAST(" + Sql + " AS REAL))";
                                break;
                            case ColumnFunction.CAST_AS_INT:
                                Sql = "CAST " + Sql + " AS INT)";
                                break;
                            default:
                                Sql = f.FunctionName.ToString() + "(" + Sql;
                                if (f.Arguments != null)
                                {
                                    foreach (var arg in f.Arguments)
                                        Sql += ", " + arg;
                                }
                                Sql += ")";
                                break;
                        }
                    }
                }

            }
            if (!string.IsNullOrEmpty(Alias) && alias)
                Sql += " AS '" + Alias + "'";

            return Sql;
        }
    }
}
