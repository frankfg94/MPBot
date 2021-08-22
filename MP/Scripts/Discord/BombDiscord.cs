using BT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BT.MP.Discord
{
    class BombDiscord : Scripts.Bomb
    {
        bool joinedAudio = false;
        private static string BOMB_BIP_PATH = Program.resourceFolderPath + "\\c4bip.mp3";
        private static string BOMB_BOOM_PATH = Program.resourceFolderPath + "\\c4boom.mp3";
        private static string BOMB_DEFUSE_PATH = Program.resourceFolderPath + "\\c4defuse.wav";

        public BombDiscord(string name, int hp, double psyLvl, EntitySize size, int dureeMillisecs) : base(name, hp, psyLvl, size, dureeMillisecs)
        {
            if(Communicator.IsInitialized)
            {
            Task.Run(() => Program.communicator.Join());
            joinedAudio = true;
            }
            else
                Console.WriteLine("Il faut d'abord initialiser le communicator");
        }


        public override void Arm(TimeSpan timeBeforeExplosion)
        {
            base.Arm(timeBeforeExplosion);
        }

        protected override void Bomb_Bip()
        {
            base.Bomb_Bip();
            Task.Run(() => Program.communicator.PlayAudio(BOMB_BIP_PATH, joinedAudio));
            Task.Run(() => Program.communicator.DisplayMsgInChat("BIP :rotating_light: "));
        }

        protected override void Detonate()
        {
            var t = new Timer
            {
                AutoReset=false,
                Interval = 5000
            };
            t.Elapsed += (s, e) =>
            {
                base.Detonate();
                Task.Run(() => Program.communicator.DisplayMsgInChat("BOOOOOOOOOOOOOOOOM"));
                Task.Run(() => Program.communicator.PlayAudio(BOMB_BOOM_PATH, joinedAudio));
            };
            t.Start();
        }

        public override void Disarm()
        {
            base.Disarm();
            var t = new Timer
            {
                AutoReset = false,
                Interval = 6000
            };
            t.Elapsed += (s, e) =>
            {
                Task.Run(() => Program.communicator.PlayAudio(BOMB_DEFUSE_PATH, joinedAudio));
                Task.Run(() => Program.communicator.DisplayMsgInChat("Bombe désamorcée!"));
            };
            t.Start();
        }

        public override void Rearm()
        {
            base.Rearm();
            Task.Run(() =>  Program.communicator.DisplayMsgInChat("La bombe semble s'être rallumée!"));
        }
    }
}
