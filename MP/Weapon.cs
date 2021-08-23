using BT.Database;
using BT.MP.Scripts.Discord;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP
{
    /// <summary>
    /// Utilisé pour les descriptions en combat ou pour les recherches dans les coffres
    /// </summary>
    public enum WeaponCategory
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

    public class Weapon : Entity
    {
        public static ObservableCollection<Weapon> GetDefaultWeapons()
        {
            if(DbRequester.Connection == null)
            {
            // Use default weapons
            var ak = new Weapon("AK-47", 20, EntitySize.Medium, 0, 30, 65, 30, 30, 3, "Arme de guerre puissante et facile à fabriquer") { wepCategory = WeaponCategory.Rifle };
            var sniper = new Weapon("Fusil de précision L96", 20, EntitySize.Medium, 0, 50, 90, 10, 10, 1, "Un fusil de précision à verrou, excellente puissance d'arrêt et précision. Malus de précision réduits") { wepCategory = WeaponCategory.Sniper };
            var minigun = new Weapon("Minigun GAU-2B", 20, EntitySize.Medium, 0, 30, 25, 30, 30, 30, "Faites pleuvoir la mort sur vos ennemis") { wepCategory = WeaponCategory.MachineGun };
            return new ObservableCollection<Weapon>
            {
                ak,
                sniper,
                minigun
            };
            } else
            {
                var data = DbRequester.ExecuteSelectQuery("SELECT * from Weapon w JOIN Precision p ON w.precision_id=p.precision_id " +
                                                                                 "JOIN Damage d ON w.damage_id=d.damage_id").Select().ToList();
                Console.WriteLine();
            }
            return null;
        }

        private double damage;
        private float precision;
        private int curAmmo;
        private int maxAmmo;
        private double rateOfFire;
        private string description;
        public WeaponCategory wepCategory;
        public bool singleUsage = false;
        public Weapon(string Name, int HP, EntitySize Size, double PsyLvl, double Damage, float Precision, int Curammo, int MaxAmmo, double rateOfFire, string Description) : base(Name, HP, Size, PsyLvl)
        {
            this.damage = Damage;
            this.precision = Precision;
            this.curAmmo = Curammo;
            this.maxAmmo = MaxAmmo;
            this.rateOfFire = rateOfFire;
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

        public float Precision { get => precision; set => precision = value; }

        //TODO : rajouter la possibilité de toucher des membres adjacents si echec
        internal void ShootAt(Entity ennemyEntity, EntityPart targetedArea, Dictionary<EntityPartType,double> precisionDict, out string description)
        {
            

            int shotSuccessfulCount = 0;
            Random r = new Random();
            
            for (int i = 0; i < rateOfFire; i++)
            {
                if( r.NextDouble()  < precisionDict[targetedArea.partType]/100 )
                {
                     shotSuccessfulCount++;
                }
            }
            if(rateOfFire > 1)
            {
                description = $"Le ** {ennemyEntity.status} ** se fait canarder d'une rafale de {wepCategory} {Name}  !";
                description += $"\n\nAu final, il est touché par **  {shotSuccessfulCount} / {rateOfFire} impacts de balles ** {Translator.endOfShootingSentence[targetedArea.partType]} .";
            }
            else
            {
                description = $"Vous tirez une fois sur le ** {ennemyEntity.status} ** avec votre {Name} {Translator.endOfShootingSentence[targetedArea.partType]}!";
            }

            if (shotSuccessfulCount > 0)
            {
                ennemyEntity.HP -= (int)(damage * shotSuccessfulCount * targetedArea.damageCoeff);
                string description2 = "";
                (ennemyEntity as IAdvancedTarget).TryInjure((int)(shotSuccessfulCount * damage * targetedArea.damageCoeff),shotSuccessfulCount, targetedArea, out description2);
                description += "\n" + description2 + "\n";
                if (!ennemyEntity.IsAlive())
                {
                    if(rateOfFire > 1)
                    {
                        description += " La rafale de balles le fait effondrer dans un cri brusque, " + ennemyEntity.Name + " n'est plus";
                    } 
                    else
                    {
                        description += " Il meurt au sol rapidement, " + ennemyEntity.Name + " n'est plus";
                    }
                }
                else
                {
                    description += "Il est toujours debout, santé restante : " + ennemyEntity.HP + " / " + ennemyEntity.maxHP;
                }
            }
            else
            {
                if(rateOfFire > 1)
                {
                    description += $"Aucune balle n'a touché {Name} c'est un échec !";
                } else
                {
                    description += "Votre tir unique n'a pas atteint l'ennemi";
                }
            }

          
        }
    }
}
