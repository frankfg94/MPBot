using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP.Scripts
{
    class RandomChest : Entity, IUsable
    {
        
        protected List<Entity> possibleEntities = new List<Entity>();
        protected Random r = new Random();
        protected bool opened = false;
        protected string password;

        public RandomChest(string Name, int HP, EntitySize size, double PsyLvl, string Password = "") : base(Name, HP, size, PsyLvl)
        {
            this.password = Password;
        }

        public string Password
        {
            get { return password; }
        }

        public virtual void Open(Entity entityThatOpen)
        {
            if(!opened)
            {
                if (this.password != "")
                {
                    Console.WriteLine("saisissez le mot de passe");
                    string pswd;
                    pswd = Console.ReadLine();
                    if (this.password == pswd)
                    {
                        opened = true;
                        Console.WriteLine(entityThatOpen.Name + " a ouvert l'objet " + Name + " et y a découvert : " + possibleEntities[r.Next(0, possibleEntities.Count)].Name);
                    }
                    else
                    {
                        Console.WriteLine("mot de passe incorrect");
                    }
                }
                
            }
        }

        public virtual void Use(Entity user)
        {
            Open(user);
        }
    }
}
