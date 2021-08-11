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
    public class Commands : ModuleBase<SocketCommandContext>
    {

        // Permission: Anyone
        [Command("myRoleInfo")]
        public async Task MyRoleInfo()
        {
            var guild = Context.Guild.ToString();
            await ReplyAsync("You are currently at the server/guild: " + guild);

            string userId = Context.User.Discriminator;
            await ReplyAsync("Your unique ID is: " + userId);

        }

        // Permission: Admin
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


        //Basic ping test
        //Permission: Anyone
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        //Reads user input
        //Permission: Anyone
        [Command("callback")]
        public async Task Callback(string message)
        {
            //This is how we grab who is sending the ticket in
            string temp = Context.User.Username;

            await Context.User.SendMessageAsync("Here is your DM message! ;) " + message + " " + temp);
        }

        //Permission: Anyone
        [Command("help")]
        public async Task Help()
        {
            string sender = Context.User.Username;

            await Context.User.SendMessageAsync($"Hello {sender}! Thank you for using SupportBot!\n  To submit a ticket, please use the command !newTicket here followed by a brief description of your troubles in parantheses. A moderator will assist you shortly.");
            await Context.User.SendMessageAsync($"Example: !newTicket \"Having trouble logging in.\"");
        }

        //Permision: Anyone
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
        
        //Permission: Mod/Admin
        [Command("viewOpenTickets")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task GetOpenTickets()
        {
            string url = "https://localhost:44322/api/LiveTickets";
            var client = new RestClient(url);
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
        
        //Permission Mod/Admin
        [Command("getTicket")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task GetTicket(int id)
        {
            string url = $"https://localhost:44322/api/LiveTickets/{id}";
            var client = new RestClient(url);
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

        //use to close a live ticket and update it with resolution
        //Permission: Mod/Admin
        [Command("closeTicket")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task CloseTicket(int id, string resolution)
        {
            Console.WriteLine("hit the route");
            string url = $"https://localhost:44322/api/LiveTickets/close/{id}";
            var client = new RestClient(url);
            var request = new RestRequest();
            string sender = Context.User.Username;
            CloseTicketDTO closing = new CloseTicketDTO()
            {
                Resolution = resolution,
                Resolver = sender,
                Closed = DateTime.Now
            };
            Console.WriteLine("made the DTO");

            request.AddJsonBody(closing);

            Console.WriteLine("added the DTO to the Body");

            //TODO: Figure out why the response is bugged
            var response = client.Put(request);

            Console.WriteLine(response.Content);

            string jsonFormatted = JValue.Parse(response.Content.ToString()).ToString(Formatting.Indented);

            await Context.User.SendMessageAsync($"Ticket has been marked as resolved:\n {jsonFormatted}");
        }
        //Send out to API with info from the command

        //Spit out info on user that triggers this
        //MemberInfo https://docs.stillu.cc/api/Discord.Rest.MemberInfo.html

        //Stretch Goals
        //Have bot register user with API server
    }
}
