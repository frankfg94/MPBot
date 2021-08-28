using System.Threading.Tasks;
using Discord.Commands;
using Discord.Audio;
using Discord;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Bot_Test;
using System;
using Discord.WebSocket;
using System.Windows;

namespace Bot_Test
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down
        private AudioService _service;
        private SocketCommandContext commandContext;
        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your botx
        public AudioModule(AudioService service)
        {
            commandContext = (SocketCommandContext)service.Context;
            _service = service;
            Console.WriteLine("Audio Module crée");
        }

        public void SetCommandContext(SocketCommandContext cm)
        {
            if (cm != null) commandContext = cm;
        }


        public async Task Test()
        {
            await Console.Out.WriteLineAsync("Test OK Console");
            try { await commandContext.Channel.SendMessageAsync("Test OK Discord"); }
            catch { await Console.Out.WriteLineAsync("ReplyAsync problème de référence"); }
            await Music1();
        }

        // You *MUST* mark these commands with 'RunMode.Async'
        // otherwise the bot will not respond until the Task times out.
        public async Task JoinCmd()
        {
            if (commandContext == null)
            {
                if(Context != null)
                {
                    await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
                } else
                {
                    Console.Error.WriteLine("Context is null, cannot play audio");
                }
            }
            else
            {
                await _service.JoinAudio(commandContext.Guild, (commandContext.User as IVoiceState).VoiceChannel);
            }
        }

        public void StopCmd()
        {
            _service.StopAudio();
        }


        [Command("music1", RunMode = RunMode.Async)]
        public async Task Music1()
        {
            try
            {
                //await ReplyAsync("Lancement fonction");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            await JoinCmd();
            await PlayCmd(songPath: @"D:/Téléchargements/koh.mp3");
        }

        List<IGuildUser> list = new List<IGuildUser>();


        private async Task SpeakDetected(ulong id, bool updated)
        {
            if (updated) await ReplyAsync("Quelqu'un parle");
        }



        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("playHQ")]
        public async Task PlayCmd()
        {
            var resPath = System.AppDomain.CurrentDomain.BaseDirectory;
            await _service.SendAudioAsync(Context.Guild, Context.Channel,$"{resPath}\\Resources\\XCOM_HQ_ACT_1.wav");
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string songPath)
        {
            if (commandContext == null)
            {
                if(Context!=null)
                {
                    commandContext = (SocketCommandContext)Context;
                }
                else {
                       var client = (DiscordSocketClient) Program._services.GetService(typeof(DiscordSocketClient));
                       var channel = (SocketGuildChannel) client.GetChannel(AdminPanel.channelID);
                       var guild = channel.Guild.Id;
                       await _service.SendAudioAsync(client.GetGuild(guild), (IMessageChannel)channel, songPath);
                }
            } else
            {
                await _service.SendAudioAsync(commandContext.Guild, commandContext.Channel, songPath);
            }
        }


        /*
        [Command("kill", RunMode = RunMode.Async)]
            public async void Kill([Remainder] string name)
            {
                var users = Context.Guild;
                for(int i = 0; i < users. ; i++)
                {

                    if (u.Username.ToUpper() == name.ToUpper() || u.Nickname.ToUpper() == name.ToUpper())
                    {
                        await u.ModifyAsync(x => 
                        .Nickname = "[MORT]" + u.Username);
                        await ReplyAsync(":scream: " + u.Username + " vient de mourir!");
                        await _service.SendAudioAsync(Context.Guild,Context.Channel,@"C:/Users/gillioen/Desktop/death.wav");

                    }
                }

            }
            */


    }

}