using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP
{
    internal class HumanArmor : Entity
    {
        //int chestArmorValue;
        //int headArmorValue;
        //int legArmorValue;
        //int armArmor;
        //int footArmor;
        //int gloveArmor;

        //public HumanArmor(string Name, int HP, double PsyLvl, EntitySize Size, int ChestArmor, int HeadArmor, int LegArmor, int ArmArmor, int footArmor, int GloveArmor) : base(Name, HP, PsyLvl, Size)
        //{
        //    this.chestArmorValue = ChestArmor;
        //    this.headArmorValue = HeadArmor;
        //    this.legArmorValue = LegArmor;
        //    this.armArmor = ArmArmor;
        //    this.footArmor = footArmor;
        //    this.gloveArmor = GloveArmor;
        //}
        public HumanArmor(string Name, int HP, EntitySize Size, double PsyLvl) : base(Name, HP, Size, PsyLvl)
        {

        }
    }
}
