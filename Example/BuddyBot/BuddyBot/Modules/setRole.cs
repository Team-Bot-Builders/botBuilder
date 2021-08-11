<<<<<<< HEAD
﻿using Discord;
using Discord.Commands;
using System;
=======
﻿using System;
>>>>>>> development
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyBot.Modules
{
<<<<<<< HEAD
    class setRole : ModuleBase<SocketCommandContext>
    {
        [Command("roleInfo")]
        public async Task RoleInfo(string userName)
        {
            var user = Context.Guild.ToString();
            if(user == null)
            {
                user = "No Guild Found";
            }

            await ReplyAsync(user);

        }

        public async Task SetRole(string userName)
        {
            //Grab the user to be changed
                //ID? IUser refference in?

            var user = Context.User;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "RoleName");
            await (user as IGuildUser).AddRoleAsync(role);

        }

        //make a few roles

        //Moderator Role: Main identifier Kick Member
        public GuildPermissions moderatorPerms = new GuildPermissions(
                //bool createInstantInvite = 
                false,
                //bool kickMembers = 
                true,
                //bool banMembers = 
                false,
                //bool administrator = 
                false,
                //bool manageChannels = 
                true,
                //bool manageGuild = 
                true,
                //bool addReactions = 
                true,
                //bool viewAuditLog = 
                true,
                //bool viewGuildInsights = 
                true,
                //bool viewChannel = 
                true,
                //bool sendMessages = 
                true,
                //bool sendTTSMessages = 
                true,
                //bool manageMessages = 
                true,
                //bool embedLinks = 
                true,
                //bool attachFiles = 
                true,
                //bool readMessageHistory = 
                true,
                //bool mentionEveryone = 
                true,
                //bool useExternalEmojis = 
                true,
                //bool connect = 
                true,
                //bool speak = 
                true,
                //bool muteMembers = 
                true,
                //bool deafenMembers = 
                true,
                //bool moveMembers = 
                false,
                //bool useVoiceActivation = 
                false,
                //bool prioritySpeaker = 
                false,
                //bool stream =
                true,
                //bool changeNickname = 
                true,
                //bool manageNicknames = 
                true,
                //bool manageRoles = 
                true,
                //bool manageWebhooks = 
                true,
                //bool manageEmojis = 
                true);



        //Moderator Role: Main identifier Ban Member, and Administrator
        public GuildPermissions adminPerms = new GuildPermissions(
                //bool createInstantInvite = 
                false,
                //bool kickMembers = 
                true,
                //bool banMembers = 
                true,
                //bool administrator = 
                true,
                //bool manageChannels = 
                true,
                //bool manageGuild = 
                true,
                //bool addReactions = 
                true,
                //bool viewAuditLog = 
                true,
                //bool viewGuildInsights = 
                true,
                //bool viewChannel = 
                true,
                //bool sendMessages = 
                true,
                //bool sendTTSMessages = 
                true,
                //bool manageMessages = 
                true,
                //bool embedLinks = 
                true,
                //bool attachFiles = 
                true,
                //bool readMessageHistory = 
                true,
                //bool mentionEveryone = 
                true,
                //bool useExternalEmojis = 
                true,
                //bool connect = 
                true,
                //bool speak = 
                true,
                //bool muteMembers = 
                true,
                //bool deafenMembers = 
                true,
                //bool moveMembers = 
                false,
                //bool useVoiceActivation = 
                false,
                //bool prioritySpeaker = 
                false,
                //bool stream =
                true,
                //bool changeNickname = 
                true,
                //bool manageNicknames = 
                true,
                //bool manageRoles = 
                true,
                //bool manageWebhooks = 
                true,
                //bool manageEmojis = 
                true);

=======
    class setRole
    {
>>>>>>> development
    }
}
