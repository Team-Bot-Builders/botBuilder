using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using DotEnvStuff;
using System.IO;
using Microsoft.Extensions.Configuration;
using BuddyBot.Modules;

namespace BuddyBot
{
    class Program
    {

        private static string BotToken;

        static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();

            var section = config.GetSection(nameof(Secrets));
            var secrets = section.Get<Secrets>();
            BotToken = secrets.BotToken;

            /*            //load in environmental stuff https://dusted.codes/dotenv-in-dotnet
                        var root = Directory.GetCurrentDirectory();
                        var dotenv = Path.Combine(root, ".env");
                        DotEnv.Load(dotenv);*/

            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();


            string token = BotToken;

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1); 

        }

        private System.Threading.Tasks.Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services) ;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("[!]", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }


    }
}
