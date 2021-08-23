using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.Database.DbTools
{
    public class ConditionsCollection
    {
        public List<Conditions> Conditions { get; set; }
        public LogicalOperator Operator { get; set; }
        public List<ConditionsCollection> List { get; set; }

        public ConditionsCollection(List<Conditions> conditions, LogicalOperator logicalOperator = LogicalOperator.NONE)
        {
            Conditions = conditions;
            Operator = logicalOperator;
        }

        public ConditionsCollection(List<ConditionsCollection> list, LogicalOperator logicalOperator = LogicalOperator.NONE)
        {
            List = list;
            Operator = logicalOperator;
        }

    }
}
