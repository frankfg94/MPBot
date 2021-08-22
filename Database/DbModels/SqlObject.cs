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

        public SqlObject(Dictionary<string, object> fields)
        {
            this.Fields = fields;
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
