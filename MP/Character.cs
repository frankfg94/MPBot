    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP
{
    class Character : Entity
    {
        private string nickname;
        private string firstName;
        private List<Entity> inventory;
        private long discordID;
        private Entity selectedEquipment;
        protected char sex;
        

        public Character(string Name, int HP, EntitySize Size, double PsyLvl, string Nickname, string FirstName, string Status, List<Entity> Inventory, long id, Entity SelectedEquipment) : base(Name, HP, Size, PsyLvl)
        {
            this.nickname = Nickname;
            this.firstName = FirstName;
            this.status = Status;
            this.inventory = Inventory;
            this.discordID = id;
            this.selectedEquipment = SelectedEquipment;
        }

        public string NickName
        {
            get { return nickname; }
        }

        public string FirstName
        {
            get { return firstName; }
        }

        public string Rank
        {
            get { return status; }
            set { this.status = value; }
        }

        public List<Entity> Inventory
        {
            get { return inventory; }
            set { this.inventory = value; }
        }

        public long DiscordID
        {
            get { return discordID; }
        }

        public Entity SelectedEquipment
        {
            get { return selectedEquipment; }
            set { selectedEquipment = value; }
        }

        public char Sex
        {
            get { return sex; }
        }

        public string Status { get => status; set => status = value; }

        public void Die()
        {
            
        }
    }

    
}
