using Bot_Test.MP.Scripts.Discord;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP.Scripts
{

    public class DeadEye 
    {

  
        public virtual async Task TargetAnEntityAsync(IAdvancedTarget e, Weapon w, List<DeadEyeModifier> modifiers)
        {
            Console.WriteLine($"Quelle partie de la cible {e.Entity.Name} souhaitez vous cibler ?");
            foreach(var area in e.GetTargetableAreas())
            {
                Console.WriteLine($" Viser la partie : {area.partType}  | efficacité : {area.damageCoeff*100}%");
            }
        }

        public virtual void Test()
        {

        }
    }
}
