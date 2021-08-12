using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuddyBot.DTO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace BuddyBot.Modules
{
    /// <summary>
    /// All of the commands that the bot will respond to
    /// </summary>
    public class Commands : ModuleBase<SocketCommandContext>
    {
        // APIToken for use with authentication
        private string APIToken;

        /// <summary>
        /// Log the bot in to the API Server to receive access token
        /// </summary>
        /// <returns> JWT</returns>
        public static string BotLogin()
        {
            string url = "https://localhost:44322/api/Users/botlogin";
            var client = new RestClient(url);
            var request = new RestRequest();

            request.AddJsonBody(new LoginDTO());

            var response = client.Post(request);

            UserDTO user = JsonConvert.DeserializeObject<UserDTO>(response.Content);
            if (user.Token != null)
            {
                Console.WriteLine("API Login Successful");
            }
            return user.Token;
        }

        /// <summary>
        /// Debug command that returns contextual information about the user from the message.
        /// </summary>
        [Command("myRoleInfo")]
        public async Task MyRoleInfo()
        {
            var guild = Context.Guild.ToString();
            await ReplyAsync("You are currently at the server/guild: " + guild);

            string userId = Context.User.Discriminator;
            await ReplyAsync("Your unique ID is: " + userId);

        }

        /// <summary>
        /// Promote a user to a moderator
        /// </summary>
        /// <param name="username">Username of individual to be promoted.</param>
        /// <permission> Administrators.</permission>
        [Command("addRole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddRole(string username)
        {

            var users = Context.Guild.GetUsersAsync();
            string foundUsername = "";
            string foundDiscriminator = "";
            var user = Context.User;

            //This targets a user correctly by username
            await foreach (var collection in users)
            {
                foreach (var person in collection)
                {
                    if(person.Username == username)
                    {
                        //Somehow store that person in a way that will let it be accessed by others
                        foundUsername = person.Username;
                        foundDiscriminator = person.Discriminator;
                        Console.WriteLine(person.Username);
                        Console.WriteLine(person.Discriminator);
                        user = (SocketUser)person;
                    }
                }
            }

            //add user to a named role that already exists
            //https://stackoverflow.com/questions/41686360/discord-net-how-do-i-grant-an-user-a-role
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Moderator");
            await (user as IGuildUser).AddRoleAsync(role);
            await Context.User.SendMessageAsync("I have made the user " + foundUsername + ":" + foundDiscriminator + " a moderator");

        }


        /// <summary>
        /// Basic debug ping test
        /// </summary>
        /// <returns> pong response message to user</returns>
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        /// <summary>
        /// Basic debug, returns input string and username of sender
        /// </summary>
        [Command("callback")]
        public async Task Callback(string message)
        {
            //This is how we grab who is sending the ticket in
            string temp = Context.User.Username;

            await Context.User.SendMessageAsync("Here is your DM message! ;) " + message + " " + temp);
        }

        /// <summary>
        /// Gives back basic information on how to support a ticket
        /// </summary>
        [Command("help")]
        public async Task Help()
        {
            string sender = Context.User.Username;

            await Context.User.SendMessageAsync($"Hello {sender}! Thank you for using SupportBot!\n  To submit a ticket, please use the command !newTicket here followed by a brief description of your troubles in parantheses. A moderator will assist you shortly.");
            await Context.User.SendMessageAsync($"Example: !newTicket \"Having trouble logging in.\"");
        }

        /// <summary>
        /// Submit a new ticket to the server
        /// </summary>
        /// <param name="description">Description of the issues that this ticket should address.</param>
        /// <returns>Confirmation message to sender that ticket was sent</returns>
        [Command("newTicket")]
        public async Task NewTicket(string description)
        {
            string url = "https://localhost:44322/api/LiveTickets";
            var client = new RestClient(url);
            var request = new RestRequest();
            string sender = Context.User.Username;
            var newTicket = new LiveTicketDTO
            {
                Created = DateTime.Now,
                Description = description,
                Requestor = sender
            };

            request.AddJsonBody(newTicket);

            var response = client.Post(request);

            Console.WriteLine(response.StatusCode.ToString() + "      " + response.Content.ToString());

            await Context.User.SendMessageAsync("Your ticket has been added to the queue!");
        }

        /// <summary>
        /// Retreive all open ticket from the Server
        /// </summary>
        /// <permission>Administrators and Moderators.</permission>
        /// <returns>Message containing all open tickets</returns>
        [Command("viewOpenTickets")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task GetOpenTickets()
        {
            if(APIToken == null)
            {
                APIToken = BotLogin();
            }
            string url = "https://localhost:44322/api/LiveTickets";
            var client = new RestClient(url);
            Console.WriteLine(APIToken);
            client.Authenticator = new JwtAuthenticator(APIToken);

            var request = new RestRequest();

            var response = client.Get(request);
            
            Console.WriteLine(response.StatusCode.ToString() + "      " + response.Content);

            string jsonFormatted = JValue.Parse(response.Content.ToString()).ToString(Formatting.Indented);

            if(Context.Channel.Name == "ticket-info")
            {
                await ReplyAsync($"These are the available tickets:\n {jsonFormatted}");
            }
            else
            {
                await Context.User.SendMessageAsync($"These are the available tickets:\n {jsonFormatted}");
            }

        }

        /// <summary>
        /// Retreive submitted ticket by id
        /// </summary>
        /// <permission>Administrators and Moderators.</permission>
        /// <returns>Message containing all open tickets</returns>
        [Command("getTicket")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task GetTicket(int id)
        {
            if (APIToken == null)
            {
                APIToken = BotLogin();
            }
            string url = $"https://localhost:44322/api/LiveTickets/{id}";
            var client = new RestClient(url);
            client.Authenticator = new JwtAuthenticator(APIToken);

            var request = new RestRequest();

            var response = client.Get(request);

            string jsonFormatted = JValue.Parse(response.Content.ToString()).ToString(Formatting.Indented);

            if (Context.Channel.Name == "ticket-info")
            {
                await ReplyAsync(jsonFormatted);
            }
            else
            {
                await Context.User.SendMessageAsync(jsonFormatted);
            }
        }

        /// <summary>
        /// Resolve a particular ticket
        /// </summary>
        /// <param name="id">Id of ticket that is being targeted to be closed.</param>
        /// <param name="resolution">Description of the resolution of the ticket issue.</param>
        /// <permission>Administrators and Moderators.</permission>
        /// <returns>Message back to user confirming the ticket was closed</returns>
        [Command("closeTicket")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task CloseTicket(int id, string resolution)
        {
            if (APIToken == null)
            {
                APIToken = BotLogin();
            }
            Console.WriteLine("hit the route");
            string url = $"https://localhost:44322/close/{id}";
            var client = new RestClient(url);
            client.Authenticator = new JwtAuthenticator(APIToken);

            var request = new RestRequest();
            string sender = Context.User.Username;
            CloseTicketDTO closing = new CloseTicketDTO()
            {
                Closed = DateTime.Now,
                Resolution = resolution,
                Resolver = sender
            };
            Console.WriteLine($"made the DTO, id: {id}, resolution: {resolution}");

            request.AddJsonBody(closing);

            Console.WriteLine("added the DTO to the Body");

            var response = client.Put(request);

            Console.WriteLine(response.Content);
            Console.WriteLine("I'm trying to make fetch happen");

            string jsonFormatted = JValue.Parse(response.Content.ToString()).ToString(Formatting.Indented);

            await Context.User.SendMessageAsync($"Ticket has been marked as resolved:\n {jsonFormatted}");
        }
    }
}
