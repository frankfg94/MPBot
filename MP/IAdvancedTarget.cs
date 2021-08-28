using System.Collections.Generic;
using static BT.MP.Weapon;

namespace Bot_Test.MP
{
    /// <summary>
    /// Cible qui peut être visée membre par membre grâce à une capacité spéciale de tir
    /// </summary>
    public interface IAdvancedTarget
    {
        List<EntityPart> GetTargetableAreas();

        /// <summary>
        /// Tente de démembrer ou de créer une hémorragie, blessure sur la cible/ Marche pour les véhicules avec incendies, explosions également
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="targetedPart"></param>
        /// <param name="descr"></param>
        void TryInjure(int dmg, int successShots, EntityPart targetedPart, out string descr);

        void GetShotAt(EntityPart targetedArea, Entity objectForAttack, Dictionary<EntityPartType,double> precisionDict,WeaponRange range, out string attackDescription);
        Entity Entity { get;  }
    }
}