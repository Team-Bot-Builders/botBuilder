using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuddyBot.DTO;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace BuddyBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        //Basic ping test
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        //Reads user input
        [Command("callback")]
        public async Task Callback(string message)
        {
            //This is how we grab who is sending the ticket in
            string temp = Context.User.Username;

            await Context.User.SendMessageAsync("Here is your DM message! ;) " + message + " " + temp);
        }

        //Adding Role "Moderator"
        [Command("moderator")]
        //[RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Moderator()
        {
            GuildPermissions newSet = new GuildPermissions(
                //bool createInstantInvite = 
                false,
                //bool kickMembers = 
                false,
                //bool banMembers = 
                false,
                //bool administrator = 
                false,
                //bool manageChannels = 
                false,
                //bool manageGuild = 
                false,
                //bool addReactions = 
                false,
                //bool viewAuditLog = 
                false,
                //bool viewGuildInsights = 
                false,
                //bool viewChannel = 
                false,
                //bool sendMessages = 
                false,
                //bool sendTTSMessages = 
                false,
                //bool manageMessages = 
                false,
                //bool embedLinks = 
                false,
                //bool attachFiles = 
                false,
                //bool readMessageHistory = 
                false,
                //bool mentionEveryone = 
                false,
                //bool useExternalEmojis = 
                false,
                //bool connect = 
                false,
                //bool speak = 
                false,
                //bool muteMembers = 
                false,
                //bool deafenMembers = 
                false,
                //bool moveMembers = 
                false,
                //bool useVoiceActivation = 
                false,
                //bool prioritySpeaker = 
                false,
                //bool stream =
                false,
                //bool changeNickname = 
                false,
                //bool manageNicknames = 
                false,
                //bool manageRoles = 
                false,
                //bool manageWebhooks = 
                false,
                //bool manageEmojis = 
                false);

            await ReplyAsync("Pong");
        }

        [Command("help")]
        public async Task Help()
        {
            string sender = Context.User.Username;

            await Context.User.SendMessageAsync($"Hello {sender}! Thank you for using SupportBot!\n  To submit a ticket, please use the command !newTicket here followed by a brief description of your troubles. A moderator will assist you shortly.");
        }

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
        [Command("viewOpenTickets")]
        public async Task GetOpenTickets()
        {
            string url = "https://localhost:44322/api/LiveTickets";
            var client = new RestClient(url);
            var request = new RestRequest();

            var response = client.Get(request);

            Console.WriteLine(response.StatusCode.ToString() + "      " + response.Content);

            string jsonFormatted = JValue.Parse(response.Content.ToString()).ToString(Formatting.Indented);

            await Context.User.SendMessageAsync($"These are the available tickets:\n {jsonFormatted}");

        }
        [Command("getTicket")]
        public async Task GetTicket(int id)
        {
            string url = $"https://localhost:44322/api/LiveTickets/{id}";
            var client = new RestClient(url);
            var request = new RestRequest();

            var response = client.Get(request);

            string jsonFormatted = JValue.Parse(response.Content.ToString()).ToString(Formatting.Indented);

            await Context.User.SendMessageAsync(jsonFormatted);
        }
        [Command("closeTicket")]
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
