using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP
{
    /// <summary>
    /// Utilisé pour les descriptions en combat ou pour les recherches dans les coffres
    /// </summary>
    internal enum WeaponCategory
    {
        Pistol,
        Magnum,
        SMG,
        Rifle,
        MachineGun,
        RocketLauncher,
        Explosive, // ex : grenade, mine
        Knife,
        Sniper,
        Crossbow,
        Bow,
        Sword,
        Spear,
        Flamethrower
    }

    internal class Weapon : Entity
    {
        private double damage;
        private double precision;
        private int curAmmo;
        private int maxAmmo;
        private double rateOfFire;
        private string description;
        public WeaponCategory wepCategory;
        public bool singleUsage = false;
        public Weapon(string Name, int HP, EntitySize Size, double PsyLvl, double Damage, double Precision, int Curammo, int MaxAmmo, double Rate, string Description) : base(Name, HP, Size, PsyLvl)
        {
            this.damage = Damage;
            this.precision = Precision;
            this.curAmmo = Curammo;
            this.maxAmmo = MaxAmmo;
            this.rateOfFire = Rate;
            this.description = Description;
        }

        public double Damage
        {
            get { return damage; }
            set { this.damage = value; }
        }



        public int CurAmmo
        {
            get { return curAmmo; }
            set { this.curAmmo = value; }
        }

        public double RateOfFire
        {
            get { return rateOfFire; }
        }

        public string Description
        {
            get { return description; }
        }

        public double Precision { get => precision; set => precision = value; }

        //TODO : rajouter la possibilité de toucher des membres adjacents si echec
        internal void ShootAt(Entity ennemyEntity, EntityPart targetedArea, out string description)
        {
            switch (targetedArea.size)
            {
                case EntitySize.Micro:
                    precision /= 3;
                    break;
                case EntitySize.Small:
                    precision /= 2;
                    break;
                case EntitySize.Medium:
                    // La précision reste la même
                    break;
                case EntitySize.Large:
                    precision += precision * 0.20;
                    break;
                case EntitySize.Huge:
                    precision += precision * 0.40;
                    break;
                case EntitySize.Gigantic:
                    precision += precision * 0.90;
                    break;
                default:
                    break;
            }

            int shotSuccessfulCount = 0;
            Random r = new Random();
            
            for (int i = 0; i < rateOfFire; i++)
            {
                if( r.NextDouble()  < precision/100 )
                {
                     shotSuccessfulCount++;
                }
            }
            description = $"Le ** {ennemyEntity.status} ** se fait canarder d'une rafale de {wepCategory} {Name}  !";
            description += $"\n\nAu final, il est touché par **  {shotSuccessfulCount} / {rateOfFire} impacts de balles ** à {targetedArea.partType}";

            if (shotSuccessfulCount > 0)
            {
                ennemyEntity.HP -= (int)(damage * shotSuccessfulCount * targetedArea.damageCoeff);
                string description2 = "";
                (ennemyEntity as IAdvancedTarget).TryInjure((int)(shotSuccessfulCount * damage * targetedArea.damageCoeff), targetedArea, out description2);
                description += "\n" + description2;
                if (!ennemyEntity.IsAlive())
                {
                    description += " La rafale de balles le fait effondrer dans un cri brusque, " + ennemyEntity.Name + " n'est plus";
                }
                else
                {
                    description += "Il est toujours debout";
                }
            }
            else
            {
                description += "Aucune balle n'a touché l'ennemi à " + targetedArea.partType + " c'est un échec !";
            }

          
        }
    }
}
