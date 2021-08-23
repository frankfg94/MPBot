using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.Database.DbTools
{
    public class Join
    {
        public string Table1 { get; set; }
        public string Table2 { get; set; }
        public string ForeignKey1 { get; set; }
        public string ForeignKey2 { get; set; }
        //public string Sql { get; set; }
        public Column Column1 { get; set; }
        public Column Column2 { get; set; }

        public Join(string table1, string table2, string fKey1, string fKey2)
        {
            Table1 = table1;
            Table2 = table2;
            ForeignKey1 = fKey1;
            ForeignKey2 = fKey2;
        }

        public Join(Column column1, Column column2)
        {
            Column1 = column1;
            Column2 = column2;
        }

        public string GetSql2()
        {
            string Sql = " JOIN " + Table2;
            Sql += " ON " + Table1 + "." + ForeignKey1 + " = " + Table2 + "." + ForeignKey2 + " ";

            return Sql;
        }

        public string GetSql()
        {
            string sql = " JOIN " + Column2.Table;
            sql += " ON " + Column1.Table + "." + Column1.Name + " = " + Column2.Table + "." + Column2.Name + " ";

            return sql;
        }
    }
}
