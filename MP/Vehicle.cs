using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP
{
    public class Vehicle : Entity, IAdvancedTarget
    {
        public Vehicle(string Name, int HP, EntitySize Size, double PsyLvl) : base(Name, HP, Size, PsyLvl)
        {
            
        }

        public Entity Entity => this;


        public void GetShotAt(EntityPart targetedArea, Entity objectForAttack, Dictionary<EntityPartType, double> precisionDict, WeaponRange range, out string attackDescription)
        {
            throw new NotImplementedException();
        }

        public List<EntityPart> GetTargetableAreas()
        {
            return parts;
        }

        public void StartEngine()
        {

        }

        public void StopEngine()
        {

        }



        public void TryInjure(int dmg, int successShots, EntityPart targetedPart, out string descr)
        {
            throw new NotImplementedException();
        }
    }
}
