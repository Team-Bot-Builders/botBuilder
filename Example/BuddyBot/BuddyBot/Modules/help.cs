using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BuddyBot.Modules
{
    class help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            string sender = Context.User.Username;

            await Context.User.SendMessageAsync("Hello {sender}! Thank you for using SupportBot!\n  To submit a ticket, please use the command !newTicket here followed by a brief description of your troubles. A moderator will assist you shortly.");
        }
    }
}
