using BT.MP.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP.Discord
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
                    BT.Program.communicator.DisplayMsgInChat("saisissez le mot de passe");
                    string pswd;
                    pswd = Console.ReadLine();
                    if (this.password == pswd)
                    {
                        opened = true;
                        BT.Program.communicator.DisplayMsgInChat(entityThatOpen.Name + " a ouvert l'objet " + Name + " et y a découvert : " + possibleEntities[r.Next(0, possibleEntities.Count)].Name);
                    }
                    else
                    {
                        BT.Program.communicator.DisplayMsgInChat("mot de passe incorrect");
                    }
                }

            }
            BT.Program.communicator.DisplayMsgInChat("");
        }

        public override void Use(Entity user)
        {
            base.Use(user);
        }

    }
}
