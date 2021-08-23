using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP
{
    class Human : Character, IAdvancedTarget
    {
        private string country;


        public Human(string Name, int HP, EntitySize Size, double PsyLvl, string Nickname, string FirstName, string Status, List<Entity> Inventory, long id, Entity SelectedEquipment, string Country, char Sex) : base(Name, HP, Size, PsyLvl, Nickname, FirstName, Status, Inventory, id, SelectedEquipment)
        {
            this.size = EntitySize.Medium;
            this.country = Country;
            this.sex = Sex;
            parts = new List<EntityPart>()
            {
                new EntityPart(2,EntityPartType.Head, new List<EntityPartType>{EntityPartType.Neck},EntitySize.Small, 0.25f),
                new EntityPart(0.5f,EntityPartType.Neck, new List<EntityPartType>{EntityPartType.Chest,EntityPartType.Head},EntitySize.Small),
                new EntityPart(0.5f,EntityPartType.Arm,new List<EntityPartType>{EntityPartType.Hand, EntityPartType.Chest},EntitySize.Medium),
                new EntityPart(0.4f,EntityPartType.Foot,new List<EntityPartType>{EntityPartType.Leg},EntitySize.Small),
                new EntityPart(2,EntityPartType.Heart,new List<EntityPartType>{EntityPartType.Chest},EntitySize.Small,0.5f),
                new EntityPart(1,EntityPartType.Chest,new List<EntityPartType>{EntityPartType.Leg,EntityPartType.Neck,EntityPartType.Arm},EntitySize.Medium),
                new EntityPart(0.75f,EntityPartType.Leg,new List<EntityPartType>{ EntityPartType.Foot,EntityPartType.Chest},EntitySize.Medium),
                new EntityPart(0.2f, EntityPartType.Hand, new List<EntityPartType> { EntityPartType.Arm },EntitySize.Small)};

            parts.AttachTo(this);
        }

       


        public string Country
        {
            get { return country; }
        }

        public Entity Entity { get => this;  }

        public List<EntityPart> GetTargetableAreas()
        {
            return parts;
        }


        public void GetShotAt( EntityPart targetedArea, Entity objectForAttack,Dictionary<EntityPartType,double> precisionDict, out string attackDescription)
        {
            attackDescription = "Erreur de Out attackDescription";
            if(objectForAttack is Weapon)
            {
                (objectForAttack as Weapon).ShootAt(this,targetedArea,precisionDict,out attackDescription);
            }
            else if( objectForAttack == null) // attaque corps à corps
            {
                if(objectForAttack is Human && (objectForAttack as Human).IsAlive()) 
                {
                    (objectForAttack as Human).Punch(targetedArea);
                }
            }
        }

        public void Punch(Entity target)
        {

        }
        private void Punch(EntityPart targetedArea)
        {

        }

        
        public void TryInjure(int dmg, int successShot, EntityPart targetedPart, out string descr)
        {
            descr = "";
            if(targetedPart.partType == EntityPartType.Hand )
            {
                if(dmg>20)
                {
                    descr = " Une des balles de haut calibre fusa et arracha la main du " + targetedPart + " à toute vitesse, il leva la tête au ciel et hurla en pleurant";
                }
                else
                {
                    descr = "La personne visée se prend une balle dans la main et recula crispée en s'appuyant contre un mur!";
                }
            }
            if(targetedPart.partType == EntityPartType.Chest)
            {
                if(dmg >= 100 )
                {
                    if(successShot == 1)
                    {
                        descr = " Le tir haute puissance traversa la cage thoracique de l'adversaire, qui éclata en éméttant des morceaux d'os sanguinolents";
                    } else if (dmg > 300)
                    {
                        descr = " Les multiples tirs dévastateurs traversent la cible ennemie de toutes part, il tombe au sol en hurlant et en perdant des morceaux de chair";
                    }
                }

            }
            if(targetedPart.partType == EntityPartType.Head)
            {
                if(dmg > 200 && successShot > 8)
                {
                    descr = " sa tête se fit progressivement réduire en bouillie sous le déluge de projectiles tiré!";
                }
                else if (dmg > 60 && successShot == 1)
                {
                    descr = "[COUP CRITIQUE] sa tête explosa comme une pastèque sous la puissance du projectile";
                }
                else if (dmg > 40)
                {
                    descr = "[COUP CRITIQUE] la rafale tirée de balles décrocha la tête de l'humain";
                }
                else
                {
                    descr = $"Le {this.status} se prit des impacts en pleine tête et tomba sans un bruit";
                }
            }

            if (targetedPart.partType == EntityPartType.Heart)
            {
                if(dmg < 10)
                {
                    descr = "Il se prend une impact léger mais au coeur! Sa mort est maintenant signée";
                }
                else
                {
                    descr = $"L'impact de balle dans l'organe vital du { this.status} le fit reculer d'un pas , il s'écroula aussitôt sur le dos en crachant une bouffée de sang" ;
                }
            }
        }
    }
}
