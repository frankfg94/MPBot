using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP
{
    public static class ExtensionEntityParts
    {
        public static void AttachTo(this List<EntityPart> parts, Entity e)
        {
            e.parts = parts; 
        }
    }

    public abstract class Entity
    {
        private string name;
        private int hp;
        protected EntitySize size;
        public List<EntityPart> parts;
        private double psyLvl;
        public string status; // Mot tag
        public readonly int maxHP;

        public Entity(string Name, int HP, EntitySize Size, double PsyLvl)
        {
            this.name = Name;
            this.hp = HP;
            this.maxHP = HP;
            this.psyLvl = PsyLvl;
            this.size = Size;
        }

        public Entity()
        {

        }


        /// <summary>
        /// L'entité n'a plus de points de vie
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            if (hp > 0)
                return true;
            return false;
        }

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        public int HP
        {
            get { return hp; }
            set { this.hp = value; }
        }

        public double PsyLvl
        {
            get { return psyLvl; }
            set { this.psyLvl = value; }
        }


    }
}
