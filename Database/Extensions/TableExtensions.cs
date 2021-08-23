using Bot_Test.Database.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.Database.Extensions
{
    public static class TableExtensions
    {
        public static SqlObject[] ToArray(this DataTable table)
        {
            SqlObject[] objects = new SqlObject[table.Rows.Count];
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> Fields = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    Fields[column.ColumnName] = row[column.Ordinal];
                }
                objects[i++] = new SqlObject(Fields);
            }
            return objects;
        }

        public static List<SqlObject> ToList(this DataTable table)
        {
            List<SqlObject> objects = new List<SqlObject>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> Fields = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                    Fields[column.ColumnName] = row[column.Ordinal];
                objects.Add(new SqlObject(Fields));
            }
            return objects;
        }

        public static List<string> ColumnToStringList(this DataTable table, string column)
        {
            List<string> list = new List<string>();
            int index = table.Columns[column].Ordinal;
            if (index != -1)
            {
                foreach (DataRow row in table.Rows)
                    list.Add(row.ItemArray[index].ToString());
            }
            return list;
        }

        public static List<int> ColumnToIntList(this DataTable table, string column)
        {
            List<int> list = new List<int>();
            int index = table.Columns[column].Ordinal;
            if (index != -1)
            {
                foreach (DataRow row in table.Rows)
                    list.Add(Convert.ToInt32(row.ItemArray[index]));
            }
            return list;
        }
    }
}
