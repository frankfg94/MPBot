using BT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP.Discord
{
    class BombDiscord : Scripts.Bomb
    {
        bool joinedAudio = false;
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
            Task.Run(() => Program.communicator.PlayAudio(@"C:\Users\franc\OneDrive\Bureau\c4bip.mp3", joinedAudio));
            Task.Run(() => Program.communicator.DisplayMsgInChat("BIP :rotating_light: "));
        }

        protected override void Detonate()
        {
            base.Detonate();
            Task.Run(() => Program.communicator.DisplayMsgInChat("BOOOOOOOOOOOOOOOOM"));
            Task.Run(() => Program.communicator.PlayAudio(@"C:\Users\franc\OneDrive\Bureau\boom.mp3", joinedAudio));
        }

        public override void Disarm()
        {
            base.Disarm();
            Task.Run(() => Program.communicator.PlayAudio(@"C:\Users\franc\OneDrive\Bureau\c4def.mp3", joinedAudio));
            Task.Run(() =>  Program.communicator.DisplayMsgInChat("Bombe désamorcée!"));
        }

        public override void Rearm()
        {
            base.Rearm();
            Task.Run(() =>  Program.communicator.DisplayMsgInChat("La bombe semble s'être rallumée!"));
        }
    }
}
