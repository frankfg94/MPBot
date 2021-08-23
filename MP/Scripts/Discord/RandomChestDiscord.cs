using Bot_Test.MP.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP.Discord
{
    class RandomChestDiscord : RandomChest
    {
        
        public RandomChestDiscord(string Name, int HP, EntitySize size, double PsyLvl, string Password = "") : base(Name, HP, size, PsyLvl, Password)
        {
        }

        public override void Open(Entity entityThatOpen)
        {
            if (!opened)
            {
                if (this.password != "")
                {
                    Bot_Test.Program.communicator.DisplayMsgInChat("saisissez le mot de passe");
                    string pswd;
                    pswd = Console.ReadLine();
                    if (this.password == pswd)
                    {
                        opened = true;
                        Bot_Test.Program.communicator.DisplayMsgInChat(entityThatOpen.Name + " a ouvert l'objet " + Name + " et y a découvert : " + possibleEntities[r.Next(0, possibleEntities.Count)].Name);
                    }
                    else
                    {
                        Bot_Test.Program.communicator.DisplayMsgInChat("mot de passe incorrect");
                    }
                }

            }
            Bot_Test.Program.communicator.DisplayMsgInChat("");
        }

        public override void Use(Entity user)
        {
            base.Use(user);
        }

    }
}
