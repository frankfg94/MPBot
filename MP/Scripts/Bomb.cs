using BT.MP.Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BT.MP.Scripts
{
    public class Bomb : Entity
    {

        // Pause Unpause
        public Timer bombTimer;
        protected TimeSpan timeBeforeExplosion;
        private readonly int dureeMillisecs;

        public Bomb(string name, int hp, double psyLvl, EntitySize size, int dureeMillisecs) : base(name, hp, size, psyLvl)
        {
            Task.Run(()=> BT.Program.communicator.DisplayMsgInChat("Une bombe a été amorcée! !!!!!!"));
            this.bombTimer = new Timer(dureeMillisecs);
            this.bombTimer.Elapsed += CurTimer_Elapsed;
            this.dureeMillisecs = dureeMillisecs;
        }

        protected virtual void Bomb_Bip()
        {
            // BIP
            Console.WriteLine("BIPBIPBIPBIP");
            timeBeforeExplosion = timeBeforeExplosion.Add(TimeSpan.FromSeconds(-dureeMillisecs/1000));
            if (!IsAlive() || timeBeforeExplosion.TotalSeconds <= 0)
            {
                Detonate();
            }
        }

        private void CurTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Bomb_Bip();
        }

        public virtual void Disarm()
        {
            bombTimer.Stop();
            Console.WriteLine("BIUUUUOUUUUU (bombe désamorcée)");
        }


        public virtual void Rearm()
        {
            if(!bombTimer.Enabled)
            {
                Console.WriteLine("La bombe se reactive!!!");
                bombTimer.Start();
            }
        }

        public virtual void Arm(TimeSpan timeBeforeExplosion)
        {
            this.timeBeforeExplosion = timeBeforeExplosion;
            bombTimer.Start();
        }

        protected virtual void Detonate()
        {
            Console.WriteLine("BOOOOOOOOOOOOOOOOOOM !! La bombe a explosé");
            bombTimer.Stop();
        }
    }
}
