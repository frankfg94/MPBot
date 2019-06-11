using BT;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BT.MP.Discord.Exclusive
{
    public class LobbyDiscord
    {
        // Identifiants de rôles
        const ulong EN_ATTENTE_ID = 587672084168310806;
        const ulong JOUEURS_ID = 587672083555942401;
        // Fin id rôles

        private int lobbyTimeSeconds;
        private int elapsedDelaySeconds;
        Timer t;
        TimeSpan remainingTime;
        public void Init(int lobbyTimeSeconds = 60, int ElapsedDelaySeconds = 20)
        {
            if(Communicator.IsInitialized)
            {
            this.lobbyTimeSeconds = lobbyTimeSeconds;
            this.elapsedDelaySeconds = ElapsedDelaySeconds;
            t = new Timer(elapsedDelaySeconds*1000);
            remainingTime = TimeSpan.FromSeconds(lobbyTimeSeconds);
            t.Elapsed += T_Elapsed;
            Task.Run(() => Program.communicator.PlayAudio(@"C:\Users\franc\OneDrive\Bureau\r6.mp3", false));
            t.Start();
            Task.Run(() =>Program.communicator.DisplayMsgInChat("--- Préparation de Mars Protocol en cours ---"));
            Task.Run(() => Program.communicator.DisplayImage("http://scd.rfi.fr/sites/filesrfi/dynimagecache/0/0/4252/2402/1024/578/sites/images.rfi.fr/files/aef_image/gign%20en%20action%202_0.jpg"
                , "Préparation"));
            Task.Run(() => Program.communicator.SetNickNamePlayers("[En attente]",Program.communicator.GetUsers()));
            Task.Run(() => Program.communicator.SetRolePlayersAsync(EN_ATTENTE_ID));
             Game g = new Game("Mars Protocol", ActivityType.Playing);
                
            }
            else
            {
                Console.WriteLine("Il faut initialiser le Communicator d'abord avec !initCom [msg]");
            }
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {

            if(remainingTime.TotalSeconds <= 0 )
            {
                    if (t != null) t.Stop();
                    Task.Run(() => Program.communicator.DisplayMsgInChat($">> Mars Protocol va commencer <<"));
                    Task.Run(() => Program.communicator.SetNickNamePlayers("", Program.communicator.GetUsers()));
                    Task.Run(() => Program.communicator.SetRolePlayersAsync(JOUEURS_ID));
            }
            else if(remainingTime.TotalSeconds > 0 )
            {
                remainingTime = remainingTime.Add(TimeSpan.FromSeconds(-elapsedDelaySeconds));
                Task.Run(() => Program.communicator.DisplayMsgInChat($":atom: :atom: {remainingTime.TotalSeconds} secondes restantes :atom: :atom:"));
            }
        }
    }
}
