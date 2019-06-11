using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP
{
    partial class Armor : Entity
    {

        private int noArmorDmgThreeshold;
        private int noUserDmgThreeshold;
        private string description;

        public Armor(string Name, int HP, EntitySize Size, double PsyLvl = 0) : base(Name, HP, Size, PsyLvl)
        {

        }
    }
}
