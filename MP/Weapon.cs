using Bot_Test.Database;
using Bot_Test.Database.Extensions;
using Bot_Test.MP.Scripts.Discord;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP
{
    public enum WeaponRange
    {
        BOUT_PORTANT = 0,
        COURTE = 1,
        MOYENNE = 2,
        LONGUE = 3,
        EXTREME = 4
    }
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
        Flamethrower,
        Shotgun
    }

    public class Weapon : Entity
    {
        public static ObservableCollection<Weapon> GetDefaultWeapons()
        {
            ObservableCollection<Weapon> wepList = new();
            var data = DbRequester.ExecuteSelectQuery("SELECT * from Weapon w JOIN Precision p ON w.precision_id=p.precision_id " +
                                                                             "JOIN Damage d ON w.damage_id=d.damage_id").ToList();
            foreach (var sqlWeapon in data)
            {
                wepList.Add(new Weapon(sqlWeapon));
            }
            return wepList;
        }



        private Dictionary<WeaponRange,double> damage;
        private Dictionary<WeaponRange, float> precision;
        private int curAmmo;
        public string sfxShootFilename = null;
        public string sfxArmFilename = null;
        private int maxAmmo;
        private double rateOfFire;
        private string description;
        public WeaponCategory wepCategory;
        public bool singleUsage = false;

        public Weapon(string Name, int HP, EntitySize Size, double PsyLvl, Dictionary<WeaponRange, double> Damage, Dictionary<WeaponRange, float> Precision, int Curammo, int MaxAmmo, double rateOfFire, string Description) : base(Name, HP, Size, PsyLvl)
        {
            this.damage = Damage;
            this.precision = Precision;
            this.curAmmo = Curammo;
            this.maxAmmo = MaxAmmo;
            this.rateOfFire = rateOfFire;
            this.description = Description;
        }

        public Weapon(Bot_Test.Database.DbModels.SqlObject sqlWeapon)
        {
            
            var fields = sqlWeapon.Fields;
            this.Name=fields["name"].ToString();
            this.description = fields["description"].ToString();
            this.rateOfFire = Convert.ToInt32(fields["firerate"]);
            this.maxAmmo = Convert.ToInt32(fields["max_ammo"]);
            this.curAmmo = maxAmmo;
            this.sfxShootFilename = fields["sfx_shoot_filename"].ToString();
            this.sfxArmFilename = fields["sfx_arm_filename"].ToString();
            switch (fields["category"])
            {
                case "Rifle":
                    this.wepCategory = WeaponCategory.Rifle;
                    break;
                case "Sniper":
                    this.wepCategory = WeaponCategory.Sniper;
                    break;
                case "Shotgun":
                    this.wepCategory = WeaponCategory.Shotgun;
                    break;
                case "Pistol":
                    this.wepCategory = WeaponCategory.Pistol;
                    break;
                case "MachineGun":
                    this.wepCategory = WeaponCategory.MachineGun;
                    break;
                case "Magnum":
                    this.wepCategory = WeaponCategory.Magnum;
                    break;
                case "SMG":
                    this.wepCategory = WeaponCategory.SMG;
                    break;
                case "Bow":
                    this.wepCategory = WeaponCategory.Bow;
                    break;
                case "Spear":
                    this.wepCategory = WeaponCategory.Spear;
                    break;
                case "RocketLauncher":
                    this.wepCategory = WeaponCategory.RocketLauncher;
                    break;
                case "Explosive":
                    this.wepCategory = WeaponCategory.Explosive;
                    break;
                case "Knife":
                    this.wepCategory = WeaponCategory.Knife;
                    break;
                case "FlameThrower":
                    this.wepCategory = WeaponCategory.Flamethrower;
                    break;
            }
            new KeyValuePair<WeaponRange, float>(WeaponRange.BOUT_PORTANT, float.Parse(fields["point_blank"].ToString()));
            this.precision = new Dictionary<WeaponRange, float>() {
                { WeaponRange.BOUT_PORTANT,float.Parse(fields["point_blank"].ToString()) },
                { WeaponRange.COURTE, float.Parse(fields["short"].ToString())  },
                { WeaponRange.MOYENNE, float.Parse(fields["medium"].ToString())  },
                { WeaponRange.LONGUE, float.Parse(fields["long"].ToString())  },
                { WeaponRange.EXTREME, float.Parse(fields["extreme"].ToString())  }
            };
            this.damage = new Dictionary<WeaponRange, double>() {
                {WeaponRange.BOUT_PORTANT, double.Parse(fields["point_blank1"].ToString())  },
                {WeaponRange.COURTE, double.Parse(fields["short1"].ToString())  },
                {WeaponRange.MOYENNE, double.Parse(fields["medium1"].ToString())  },
                {WeaponRange.LONGUE, double.Parse(fields["long1"].ToString())  },
                {WeaponRange.EXTREME, double.Parse(fields["extreme1"].ToString()) }
            };

        }

        public Dictionary<WeaponRange, double> Damage
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

        public Dictionary<WeaponRange, float> Precision { get => precision; set => precision = value; }

        //TODO : rajouter la possibilité de toucher des membres adjacents si echec
        internal void ShootAt(Entity ennemyEntity, EntityPart targetedArea, Dictionary<EntityPartType,double> precisionDict, WeaponRange range, out string description)
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
                ennemyEntity.HP -= (int)(damage[range] * shotSuccessfulCount * targetedArea.damageCoeff);
                string description2 = "";
                (ennemyEntity as IAdvancedTarget).TryInjure((int)(shotSuccessfulCount * damage[range] * targetedArea.damageCoeff),shotSuccessfulCount, targetedArea, out description2);
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
                    description += "Il est toujours en vie, santé restante : " + ennemyEntity.HP + " / " + ennemyEntity.maxHP;
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
