using BT.MP.Scripts.Discord;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.MP.Discord
{
    public class Communicator : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService command;
        private static  SocketCommandContext commandContext;
        private SocketReaction lastImportantReaction;


        internal List<ulong> msgIdsToWatch = new List<ulong>();
        public static TaskCompletionSource<bool> tcs = null;

        public static bool IsInitialized { get { return commandContext != null; } }

        public Communicator (global::Discord.WebSocket.DiscordSocketClient _client, CommandService _commands, AudioService service)
        {
            this.client = _client;
            this.command = _commands;
        }

        public IReadOnlyCollection<SocketGuildUser> GetUsers() 
        {
            return commandContext.Guild.Users;
        }

        public async Task<global::Discord.Rest.RestUserMessage> DisplayEmbedInChat(Embed embed)
        {
            return await commandContext.Channel.SendMessageAsync(null,false, embed);
        }

        [Command("msg", RunMode = RunMode.Async)]
        public  async Task DisplayMsgInChat(string msg = "",bool robotSpeak=false)
        {
            if (commandContext == null && Context != null)
            {   
                await ReplyAsync(">> synchronsation effectuée <<");
                commandContext = Context;
            }

            await  Console.Out.WriteLineAsync("Tentative d'affichage dans discord de :  " + msg);
            if (Context == null)
                await commandContext.Channel.SendMessageAsync(msg,robotSpeak);
            else
                await ReplyAsync(msg,robotSpeak);
        }

        internal SocketReaction WaitForReaction(IAnswerable script,  RestUserMessage msg, int timeoutMs = -1)
        {
            tcs = new TaskCompletionSource<bool>();
            msgIdsToWatch = new List<ulong>();
            if (msg  == null)
                throw new Exception("Le message est null");
            msgIdsToWatch.Add(msg.Id);
            var task = tcs.Task; 
            task.Wait(timeoutMs); // On attend l'event de la reaction
            if (tcs.Task.IsCompleted)
            {
                msgIdsToWatch.Remove(msg.Id);
                Console.WriteLine("Réponse obtenue WaitForReaction()");
                return lastImportantReaction as SocketReaction;
            }
            else
            {
                Console.WriteLine("Timeout");
                return null;
            }
        }



        [Command("init", RunMode = RunMode.Async)]
        public async Task Initialize()
        {
            Console.WriteLine(" Initialisé : " +commandContext==null);
            Console.WriteLine(" Contexte existe :  "+  Context==null);

            if (commandContext == null && Context != null)
            {
                await ReplyAsync(">> synchronsation effectuée <<");
                commandContext = Context;
                Console.WriteLine("Synchronisation effectuée avec succès");
            }
            if(commandContext==null)
                Console.Error.WriteLine("Echec de synchronisation");
        }

        internal void SetNickNamePlayers(string v, IReadOnlyCollection<SocketGuildUser> users)
        {
            
            foreach (var user in users)
            {
                user.ModifyAsync(x => x.Nickname =  v + " " + user.Username );
            }
        }

        public async Task ParseReaction(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if(msgIdsToWatch.Count>0 && !reaction.User.Value.IsBot && msgIdsToWatch.Contains(msg.Id))
            {
                if (tcs != null)
                {
                    lastImportantReaction = reaction;
                    tcs.SetResult(true);
                }
            }
        }

        [Command("setRole", RunMode = RunMode.Async)]
        public async Task SetRolePlayersAsync(ulong roleId)
        {
            var role = commandContext.Guild.GetRole(roleId);
           // await role.ModifyAsync(x => x.Position = 0); // Parce que ce systeme relou ne dit pas que si la hierarchie n'est pas assez haute, alors il ne se passe rien, rien n'est affiché dans la console, rien ne change juste à cause de ça pour tout ce qui est l'attribution de rôles!
            await role.ModifyAsync(x=>x.Hoist = true); // On affiche ça séparemment

            if (role == null)
                Console.WriteLine("Role non trouvé");

            foreach (var user in commandContext.Guild.Users)
            {
                if (!user.IsBot && user.Id != 353243323592605699)
                {
                    Console.WriteLine("Ajout pour : " + user.Username + "du role:" + role.Name  );
                    await user.ModifyAsync(x => x.Roles = new List<IRole> { role });
                }
            }
        }

        [Command("imgDiscordMP", RunMode = RunMode.Async)]
        public async Task DisplayImage(string imgUrl,string footerMsg)
        {
            EmbedBuilder embedBuilder = new EmbedBuilder
            {
                Title = "Mars Protocol",
                ImageUrl = imgUrl,
                Color = Color.DarkBlue
            };
            EmbedFooterBuilder e = new EmbedFooterBuilder
            {
                Text = footerMsg
            };
            embedBuilder.Footer = e;
            await commandContext.Channel.SendMessageAsync(string.Empty, false, embedBuilder.Build());
        }

        public void StopAudio()
        {
            Console.WriteLine("Tentative d'arrêt");
            foreach (Process p in Process.GetProcessesByName("ffmpeg"))
            {
                p.Close();
            }
            Console.WriteLine("Succès");
        }

        [Command("sampleAudio", RunMode = RunMode.Async)]
        public async Task PlayAudio(string path, bool joinedAudio)
        {
            if (commandContext == null)
                commandContext = Context;
            AudioModule am = new AudioModule((AudioService)BT.Program._services.GetService(typeof(AudioService)));
            am.SetCommandContext(commandContext);
            try
            {
                if(!joinedAudio)
                {
                    Console.WriteLine("Pas besoin de rejoindre l'audio");
                    await am.JoinCmd();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Echec audio bombe");
            }

                await am.PlayCmd(path);
            
        }


        /// <summary>
        /// Tente de connecter le bot au channel Audio où se trouve actuellement les joueurs
        /// </summary>
        /// <returns></returns>
        [Command("comJoinAudio", RunMode = RunMode.Async)]
        public async Task Join()
        {
            if (commandContext == null)
                commandContext = Context;
            AudioModule am = new AudioModule((AudioService)BT.Program._services.GetService(typeof(AudioService)));
            am.SetCommandContext(commandContext);
            try
            {
                await am.JoinCmd();
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Echec audio join");
            }
        }


        [Command("com", RunMode = RunMode.Async)]
        public  async Task Test()
        {
            Console.WriteLine("Testing");
            try
            {
               await DisplayMsgInChat("Coucou ceci est un test");
            }
            catch (Exception)
            {
                Console.WriteLine("Test Failed!");
            }
        }
    }
}
