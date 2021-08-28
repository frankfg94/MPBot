using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Bot_Test.Database;
using Microsoft.Extensions.DependencyInjection;
using Bot_Test.Database.DbModels;
using System.Collections.Generic;
using Bot_Test.Database.Extensions;

namespace BT
{

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public class Program
    {
        
        public static BT.MP.Discord.Communicator communicator { get; set; }
        public static AdminPanel adminPanel;
        public static bool MarsProtocolEnabled;
        private  DiscordSocketClient _client;
        private CommandService _commands;
        public static readonly string resourceFolderPath = System.AppDomain.CurrentDomain.BaseDirectory + "Resources";

        [STAThread] // semble ne rien faire
        static void Main(string[] args) 
        {
            try
            {
                DbRequester.SetConnection();
            }
            catch (Exception ex)
            {
                PrintException(ex);
            }
            Console.WriteLine("Starting ...\n");
           
            new Program().RunBotAsync().GetAwaiter().GetResult();
        }
        //   public void InitDiscordBot() => new MainWindow().RunBotAsync().GetAwaiter().GetResult();

        public static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        // Access context features such as the discord client
        static public IServiceProvider _services;

        //string[] lines = File.ReadAllLines("token.txt");
        private string botToken;



        public IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
             .AddSingleton(_client)
             .AddSingleton(_commands)
             .AddSingleton(audioService)
             .AddSingleton(new AudioModule(audioService))
             .BuildServiceProvider();
        }

         public static AudioService audioService;
        IServiceProvider serviceProvider;
        [STAThread] // semble ne rien faire
        public async Task RunBotAsync()
        {

            //_services = new ServiceCollection().AddSingleton(new AudioService());
            audioService = new AudioService();
            _commands = new CommandService();
            _client = new DiscordSocketClient();
            _client.Log += Log;

            _services = BuildServiceProvider(); 

            await RegistercommandAsync();

            _client.UserJoined += AnnounceJoinedUser; //Check if userjoined
            _client.UserVoiceStateUpdated += VoiceUpdate;
            Ping p = new Ping().Init(_client);

            Thread t = new(new ThreadStart(() =>
            {
                adminPanel = new AdminPanel();
                 
                adminPanel.InitializeComponent();
                _client.ReactionAdded += p.ReactionParse;
                adminPanel.Show();
                Dispatcher.Run();
            }));


            await _client.LoginAsync(TokenType.Bot, File.ReadAllText("botToken.txt").Trim());
            await _client.StartAsync();

            communicator = new MP.Discord.Communicator(_client, _commands, audioService);
            //Mars protocol
            _client.ReactionAdded += communicator.ParseReaction;

            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();

            await Task.Delay(-1);

            // event subscription  
        }


        public async Task VoiceUpdate(SocketUser user, SocketVoiceState state, SocketVoiceState state2) //welcomes New Players
        {
            //var channel = _client.GetChannel(370666551306616845) as SocketTextChannel; //gets channel to send message in
            //await channel.SendMessageAsync("Audio mis à jour pour " + user + "\nmuté :" + state.IsMuted + "\nChaine :" + state.VoiceChannel);
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user) //welcomes New Players
        {
            var channel = _client.GetChannel(370666551306616845) as SocketTextChannel; //gets channel to send message in
            await channel.SendMessageAsync("Bienvenue! " + user.Mention + " sur le serveur!"); //Welcomes the new useu
            if (user.Username.Contains("anon"))
            {
                await channel.SendMessageAsync("Vous êtes Jeremy Martin il me semble, le spécialiste de JV.COM"); //Welcomes the new user
                await user.ModifyAsync(x =>
                {
                    x.Nickname = "[JV]" + x.Nickname;

                }
                );
            }
            else if (user.Username.Contains("Mastermanga"))
            {
                await channel.SendMessageAsync("Ah, Yoann Torrado, je vous attendais! "); //Welcomes the new user
                await user.ModifyAsync(x =>
                {
                    x.Nickname = ">>" + user.Username + "<<";
                }
                );
            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Fonction qui détecte chaque message reçu dans discord et lance l'événement HandleCommandAsync pour le traiter
        /// </summary>
        /// <returns></returns>
        public async Task RegistercommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(),_services);
        }

        static Ping p;
        /// <summary>
        ///Traitement du message par l'API de Discord
        /// </summary>
        /// <param name="arg">Désigne le contenu texte du message lui-même</param>
        /// <returns></returns>
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;
            int argPos = 0;

            
            if (message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos) || MarsProtocolEnabled )
            {
                if(MarsProtocolEnabled)
                {
                    //if (p == null) p = new Ping(null).Init(_client);
                    Console.WriteLine("CONTENT:" + arg.Content);
                    await p.MP(arg.Content);
                }
                var context = new SocketCommandContext(_client, message);
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
            else
            {
                Console.WriteLine("MP non activé");
            }

        }

    
    }
}
