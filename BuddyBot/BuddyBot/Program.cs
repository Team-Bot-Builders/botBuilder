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
using BuddyBot.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization;

namespace BuddyBot
{
    class Program
    {
        // Set up tokens
        private static string BotToken;
        private static string APIToken;


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

            APIToken = BotLogin();

            new Program().RunBotAsync().GetAwaiter().GetResult();
        }
        
        // Dependency Injection
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        /// <summary>
        /// The list of things that the bot will conitnually attempt to do (inputs allowing)
        /// </summary>
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

        /// <summary>
        /// Setup logging message pipeline
        /// </summary>
        private System.Threading.Tasks.Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Read in a command and progress through the received commands
        /// </summary>
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services) ;
        }

        /// <summary>
        /// Handling a single command sent to the bot
        /// </summary>
        /// <param name="arg">Message that the bot reads from a server it is attached to.</param>
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("[!]", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    await context.User.SendMessageAsync(result.ErrorReason);
                }
            }
        }

        /// <summary>
        /// Log the bot in to the API server to receive a token
        /// </summary>
        /// <returns>Jason Web Token</returns>
        public static string BotLogin()
        {
            string url = "https://localhost:44322/api/Users/botlogin";
            var client = new RestClient(url);
            var request = new RestRequest();

            request.AddJsonBody(new LoginDTO());

            var response = client.Post(request);

            string jsonFormatted = JValue.Parse(response.Content.ToString()).ToString(Formatting.Indented);
            Console.WriteLine(jsonFormatted);

            UserDTO user = JsonConvert.DeserializeObject<UserDTO>(response.Content);
            if(user.Token != null)
            {
                Console.WriteLine("API Login Successful");
            }
            return user.Token;
        }
    }
}
