using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chika.Modules
{
    public class TestModules : ModuleBase
    {
        [Command("Test")]
        public async Task TestAsync()
        {
            try
            {
                var embed = new EmbedBuilder();
                embed.WithAuthor("Testing");
                embed.WithDescription("This is a  test message.");
                await Context.Channel.SendMessageAsync("", false embed: embed.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
