using System.Collections.Generic;

namespace Bot_Test.MP
{
    public class EntityPart
    {
        public float damageCoeff;
        public EntityPartType partType;
        private List<EntityPartType> adjPartsType; // Parties directement en contact
        float progressiveDmgSpeed; // Dégats de saignement, feu
        Entity entity; // Entity auquel la partie du corps est rattachée
        public readonly EntitySize size;

        public EntityPart(float damageCoeff, EntityPartType part, List<EntityPartType> connectedParts, EntitySize size, float progressiveDmgSpeed = 0)
        {
            this.damageCoeff = damageCoeff;
            this.partType = part;
            this.progressiveDmgSpeed = progressiveDmgSpeed;
            this.adjPartsType = connectedParts;
            this.size = size;
        }



        public void Dismember()
        {

        }
    }
}