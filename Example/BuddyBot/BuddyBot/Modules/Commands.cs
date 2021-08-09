using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

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
            GuildPermissions newSet = GuildPermissions(bool createInstantInvite = false, bool kickMembers = false, bool banMembers = false, bool administrator = false,
                        bool manageChannels = false, bool manageGuild = false, bool addReactions = false, bool viewAuditLog = false,
                        bool viewGuildInsights = false, bool viewChannel = false, bool sendMessages = false, bool sendTTSMessages = false,
                        bool manageMessages = false, bool embedLinks = false, bool attachFiles = false, bool readMessageHistory = false,
                        bool mentionEveryone = false, bool useExternalEmojis = false, bool connect = false, bool speak = false,
                        bool muteMembers = false, bool deafenMembers = false, bool moveMembers = false, bool useVoiceActivation = false,
                        bool prioritySpeaker = false, bool stream = false, bool changeNickname = false, bool manageNicknames = false,
                        bool manageRoles = false, bool manageWebhooks = false, bool manageEmojis = false);
            await ReplyAsync("Pong");
        }

        //Send out to API with info from the command

        //Spit out info on user that triggers this
        //MemberInfo https://docs.stillu.cc/api/Discord.Rest.MemberInfo.html

        //Stretch Goals
        //Have bot register user with API server

    }
}
