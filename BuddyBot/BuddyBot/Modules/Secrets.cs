using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyBot.Modules
{
    /// <summary>
    /// Helper class to define the shape of envirnmental variables to be loaded in
    /// </summary>
    public class Secrets
    {
        public const string SecretToken = "SecretToken";
        public string BotToken { get; set; }

    }
}
