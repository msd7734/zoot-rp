using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;

namespace ZootRPClient
{
    class Program
    {
        private static readonly string CON_TOKEN = "MjQzMTkxNDQ2MjUxNzY1NzYx.CvuR_w.6MvuzUIQNqz88kQZ3VvrRaJw4zY";

        static void Main(string[] args)
        {
            DiscordClient client = new DiscordClient();
            client.MessageReceived += async (s, e) =>
            {
                if (!e.Message.IsAuthor && e.Message.Text.Substring(0, 3) == "!RP")
                    await e.Channel.SendMessage(String.Format("Hello there, {0}!", e.User.Name));
            };

            client.ExecuteAndWait(async () =>
            {
                await client.Connect(CON_TOKEN, TokenType.Bot);
            });
        }
    }
}
