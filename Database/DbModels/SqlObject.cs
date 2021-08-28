using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.Database.DbModels
{
    public class SqlObject
    {
        public Dictionary<string, object> Fields { get; set; }
        public string NameColumn { get; set; }

        public SqlObject(Dictionary<string, object> fields, string name = null)
        {
            this.Fields = fields;
            this.NameColumn = name;
        }

        public T GetField<T>(string fieldName) {
            return (T)Convert.ChangeType(Fields[fieldName], typeof(T));
        }
        public SqlObject() { }

        public override string ToString()
        {
            string str = "";
            foreach (var column in Fields.Keys)
            {
                str += Fields[column].ToString() + "; ";
            }
            return str;
        }
    }
}
