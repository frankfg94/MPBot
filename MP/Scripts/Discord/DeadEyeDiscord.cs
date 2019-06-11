using BT;
using BT.MP;
using BT.MP.Scripts;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Test.MP.Scripts.Discord
{
    class DeadEyeDiscord : DeadEye, IAnswerable
    {
        public List<ulong> AnsweredMessagesIds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ParseAnswer(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            throw new NotImplementedException();
        }

        public override async Task TargetAnEntityAsync(IAdvancedTarget e)
        {
            var wp = new Weapon("AK-47", 20, EntitySize.Medium, 0, 30, 65, 30, 30, 3, "arme de guerre puissante et facile à fabriquer") { wepCategory = WeaponCategory.Rifle };
            await Task.Run(async () => await Program.communicator.DisplayMsgInChat($"------------------------------------------------------------------------------------------------------------"));
            await Task.Run(async () => await Program.communicator.DisplayMsgInChat($"Quelle partie de {e.Entity.Name} visez-vous ?",true));
            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.Title = "Système De Tir Avancé | "+wp.Name;
            embedBuilder = embedBuilder.WithColor(Color.Orange);
            embedBuilder.Footer = new EmbedFooterBuilder { Text = "v1.0" };

            var areas = e.GetTargetableAreas();
            var bodyPartsCount = areas.Count;
            for (int i = 0; i < bodyPartsCount; i++)
            {
                embedBuilder.Fields.Add(new EmbedFieldBuilder { Name = ":large_blue_circle: Zone " + Translator.alphabetUnicode[i] + embedBuilder.Author , Value = $"  Viser la partie : {areas[i].partType}  | Efficacité : {areas[i].damageCoeff * 100}% | Taille :  {areas[i].size}" });
            }

            var msg=  Task.Run(async ()=>await Program.communicator.DisplayEmbedInChat(embedBuilder.Build()));
            Emoji[] reactions = new Emoji[bodyPartsCount];
            for (int i = 0; i < bodyPartsCount; i++)
            {
                Console.WriteLine("Ajout emoji "+ Translator.alphabetUnicode[i]);
                reactions[i] = new Emoji(Translator.alphabetUnicode[i]);
            }
            await Task.Run(async ()=> (await msg).AddReactionsAsync(reactions));
            SocketReaction reaction = Program.communicator.WaitForReaction(this,await msg );
            for (int i = 0; i < Translator.alphabetUnicode.Count; i++)
            {
                Console.WriteLine($"Verif {Translator.alphabetUnicode[i]} == {reaction.Emote.Name}");
               if(Translator.alphabetUnicode[i] == reaction.Emote.Name)
               {
                    string description = "Erreur description système de tir";
                    e.GetShotAt(e.GetTargetableAreas()[i],wp, out description);
                    await Task.Run(async () => await Program.communicator.DisplayMsgInChat(description));
                    break;
               }
            }
            await Task.Run(async () => await Program.communicator.DisplayMsgInChat($"------------------------------------------------------------------------------------------------------------"));
        }

        public override void Test()
        {
            Human h = new Human("Marc",100,EntitySize.Medium,0,"Joe","Joe","Caporal",null,-1,null,"USA",'M');
            h.status = "fonctionnaire";
            Task.Run(()=>TargetAnEntityAsync(h));
        }


    }
}
