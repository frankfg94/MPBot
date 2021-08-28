using Bot_Test.Database;
using Bot_Test.Database.DbModels;
using Bot_Test.Database.Extensions;
using BT.MP;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test
{
    /**
     * A real player in discord for the roleplay game, must be registered in the database with a discord_id first.
     * Use the DbBrowser tool for example to add a row with the discord_id column pointing to a real discord user in the server.
     */
    public class Player
    {
        public string firstname;
        public string lastname;
        public string codename;
        public int hp;
        public int maxHp;

        private Inventory inventory;
        public Player(ulong discordId)
        {
            var playerDict = DbRequester.ExecuteSelectQuery($"SELECT discord_id from Character WHERE discord_id={discordId}").ToList()[0];
            this.firstname = playerDict.GetField<string>("firstname");
            this.lastname = playerDict.GetField<string>("lastname");
            this.codename = playerDict.GetField<string>("codename");
            this.hp = playerDict.GetField<int>("hp");
            this.maxHp = playerDict.GetField<int>("max_hp");
        }

        public Player(SocketGuildUser user) : this(user.Id)
        {
            
        }

        internal Embed GetPassiveSkillsEmbed()
        {
            throw new NotImplementedException();
        }
    }
}
