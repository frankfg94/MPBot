using BT;
using BT.MP.Discord;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.MP.Discord.Exclusive
{
    public class EndMPDiscord
    {
        public void End()
        {
            if(Communicator.IsInitialized)
            {
                foreach (var u in Program.communicator.GetUsers())
                {
                    u.ModifyAsync(x => x.Nickname = u.Username);
                    u.ModifyAsync(x => x.Roles = new List<IRole> { });
                }
            }
            else
            {
                Console.WriteLine("Il faut d'abord initialiser le communicator");
            }
        }
    }
}
