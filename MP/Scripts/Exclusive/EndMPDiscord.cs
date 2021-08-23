using Bot_Test;
using Bot_Test.MP.Discord;
using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Test.MP.Discord.Exclusive
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
