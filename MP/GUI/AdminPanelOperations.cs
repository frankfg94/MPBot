using BT.MP.Discord;
using BT.MP.Discord.Exclusive;
using BT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BT.MP.GUI
{
    public class AdminPanelOperations
    {

        BombDiscord b;
        
        internal void ArmSampleBomb()
        {
            Console.WriteLine("Starting the bomb");
            b = new BombDiscord("C4", 10, 0, BT.MP.EntitySize.Small, 5000);
            b.Arm(TimeSpan.FromSeconds(40));
        }

        internal void DisarmSampleBomb()
        {
            Console.WriteLine("Disarming the bomb");
            b.Disarm();
        }

        internal void RearmSampleBomb()
        {
            Console.WriteLine("Rearming the bomb");
            b.Rearm();
        }

        internal void StartLobby()
        {
            Console.WriteLine("Starting the lobby");
            new LobbyDiscord().Init();

        }

        /// <summary>
        /// Ne marche pas
        /// </summary>
        internal void StopAudio()
        {
           Task.Run(()=> Program.communicator.StopAudio());
        }

        internal void InitContextVar()
        {
            Task.Run(() => Program.communicator.SetCurChannelAsContext());
        }

        
    }
}
