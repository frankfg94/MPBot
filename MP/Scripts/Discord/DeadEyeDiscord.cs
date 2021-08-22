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

        public override async Task TargetAnEntityAsync(IAdvancedTarget e, Weapon wp, List<DeadEyeModifier> modifiers)
        {
            await Task.Run(async () => await Program.communicator.DisplayMsgInChat($"------------------------------------------------------------------------------------------------------------"));
            await Task.Run(async () => await Program.communicator.DisplayMsgInChat($"Quelle partie de {e.Entity.Name} visez-vous ?",true));
            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.Title = "Système De Tir Avancé | "+wp.Name;
            embedBuilder = embedBuilder.WithColor(Color.Orange);
            embedBuilder.AddField("Description de l'arme : ", wp.Description);
            embedBuilder.AddField("Précision de base de l'arme : ", wp.Precision + "%");
            embedBuilder.Footer = new EmbedFooterBuilder { Text = "v1.0" };

            var areas = e.GetTargetableAreas();
            var precisionDict = CalculatePrecisions(areas,wp);
            if(modifiers!=null && modifiers.Count > 0)
            {
                foreach (var m in modifiers)
                {
                    if (m.precisionChange > 0)
                    {
                        embedBuilder.AddField($":star: Bonus ajouté : {m.precisionChange} %", "Raison :" + m.message);
                    }
                    else
                    {
                        embedBuilder.AddField($":octagonal_sign: Malus pris en compte : {m.precisionChange} %", "Raison :" + m.message);
                    }
                    foreach (var k in precisionDict.Keys)
                    {
                        precisionDict[k] += m.precisionChange;
                        
                    }
                }
            }
            var bodyPartsCount = e.Entity.parts.Count;
            for (int i = 0; i < bodyPartsCount; i++)
            {
                embedBuilder.Fields.Add(new EmbedFieldBuilder { Name = "Choix " + Translator.alphabetUnicode[i] + embedBuilder.Author , Value = $"  Viser la partie : {areas[i].partType}  | Efficacité : {areas[i].damageCoeff * 100}% | Taille :  {areas[i].size} | Précision : {precisionDict[areas[i].partType]} %" });
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
                    e.GetShotAt(e.GetTargetableAreas()[i],wp,precisionDict, out description);
                    await Task.Run(async () => await Program.communicator.DisplayMsgInChat(description));
                    break;
               }
            }
            await Task.Run(async () => await Program.communicator.DisplayMsgInChat($"------------------------------------------------------------------------------------------------------------"));
        }
        private Dictionary<EntityPartType, double> CalculatePrecisions(List<EntityPart> parts, Weapon weapon)
        {
            Dictionary<EntityPartType, double> dict = new();
            foreach (var area in parts)
            {
                float precision = weapon.Precision;
                switch (area.size)
                {
                    case EntitySize.Micro:
                        if (weapon.wepCategory == WeaponCategory.Sniper)
                        {
                            precision /= 2;
                        }
                        else
                        {
                            precision /= 3;
                        }
                        break;
                    case EntitySize.Small:
                        if (weapon.wepCategory == WeaponCategory.Sniper)
                        {
                            // No precision malus for snipers
                            precision /= 1.5f;
                        } 
                        else
                        {
                            precision /= 2;
                        }
                        break;
                    case EntitySize.Medium:
                        // La précision reste la même
                        break;
                    case EntitySize.Large:
                        precision *= 1.20f;
                        break;
                    case EntitySize.Huge:
                        precision *= 1.40f;
                        break;
                    case EntitySize.Gigantic:
                        precision *= 2;
                        break;
                    default:
                        break;
                }
                dict.Add(area.partType,Math.Round(precision,1));
            }
            return dict;
        }

        private Dictionary<EntityPartType,double> CalculatePrecisions(IAdvancedTarget target, Weapon weapon)
        {
            return CalculatePrecisions(target.Entity.parts, weapon);
        }

        internal async void TargetWithPanelData(AdminPanel adminPanel)
        {
            var h = new Human(adminPanel.enemy_name_Tbox.Text,
                100,
                EntitySize.Medium,
                0,
                "Joe",
                "Joe",
                adminPanel.enemy_job_Tbox.Text,
                null,
                -1,
                null,
                "USA",
                'M');
            DeadEyeModifier modifier = null;
            if (int.TryParse(adminPanel.precision_Tbox.Text, out int precisionBonusOrMalus))
            {
                 modifier = new DeadEyeModifier(precisionBonusOrMalus, adminPanel.precision_msg_Tbox.Text);
            }
             await TargetAnEntityAsync(
                h,
                adminPanel.weapon_Combobox.SelectedItem as Weapon,
                new List<DeadEyeModifier>() { modifier });
        }

        public override void Test()
        {
            var ak = new Weapon("AK-47", 20, EntitySize.Medium, 0, 30, 65, 30, 30, 3, "arme de guerre puissante et facile à fabriquer") { wepCategory = WeaponCategory.Rifle };
            Human h = new Human("Marc",100,EntitySize.Medium,0,"Joe","Joe","Caporal",null,-1,null,"USA",'M');
            h.status = "fonctionnaire";
            Task.Run(()=>TargetAnEntityAsync(h,ak,null));
        }


    }
}
