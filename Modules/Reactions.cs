using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Chika.Common;
using Discord;
using Discord.Commands;

namespace Chika.Modules
{
    public class Reactions : ChikaModule
    {
        public Random rnd = new Random();

        [Command("kiss")]
        [Description("slap @User")]
        [RequireBotPermission(GuildPermission.AttachFiles)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task KissAsync(IGuildUser user)
        {
            var kisses = new List<string>()
            {
                "https://tenor.com/view/anime-kiss-love-sweet-gif-5095865",
                "https://tenor.com/view/kiss-kiss-girl-kiss-girl-anime-anime-couple-gif-14374955",
                "https://i.imgur.com/II1bakc.gif",
                "https://i.imgur.com/II1bakc.gif",
                "https://i.imgur.com/7GhTplD.gif",
                "https://i.imgur.com/I159BUo.gif",
                "https://i.imgur.com/Uow8no2.gif",
                "https://i.imgur.com/xyjeEAJ.gif",
                "https://i.imgur.com/1IuyOxK.gif",
            };
            try
            {
                int index = rnd.Next(kisses.Count);
                var embed = new EmbedBuilder();
                embed.WithAuthor($"{Context.User.Username} kissed {user}");
                embed.WithImageUrl(kisses[index]);
                embed.WithFooter($"[DEBUG] : {kisses[index]}");
                await Context.Channel.SendMessageAsync("", false, embed: embed.Build()).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [Command("cry")]
        [Description("cry @User")]
        [RequireBotPermission(GuildPermission.AttachFiles)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task CryAsync()
        {
            var cries = new List<string>()
            {
                "https://i.imgur.com/lrxboSa.gif",
                "https://i.imgur.com/yiM5Afb.gif",
                "https://i.imgur.com/QDFK6R2.gif",
                "https://i.imgur.com/ljQLobh.gif",
                "https://i.imgur.com/c2imqbf.gif",
                "https://i.imgur.com/8E3LyJM.gif",
                "https://i.imgur.com/Oi9KQxC.gif",
                "https://i.imgur.com/rS6v1Nb.gif",
                "https://i.imgur.com/N0NuRfn.gif",
                "https://i.imgur.com/yBp7EqZ.gif",
                "https://i.imgur.com/5RDiECJ.gif",
                "https://i.imgur.com/1wZoyAd.gif",
                "https://i.imgur.com/WgWLNAt.gif",
                "https://i.imgur.com/0Q9jndm.gif",
            };
            try
            {
                int index = rnd.Next(cries.Count);
                var embed = new EmbedBuilder();
                embed.WithAuthor($"{Context.User.Username} is sad and want's to cry. :frown:");
                embed.WithImageUrl(cries[index]);
                embed.WithFooter($"[DEBUG] : {cries[index]}");
                await Context.Channel.SendMessageAsync("", false, embed: embed.Build()).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("happy")]
        [Description("happy @User")]
        [RequireBotPermission(GuildPermission.AttachFiles)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task HappyAsync()
        {
            var happy = new List<string>()
            {
                "https://i.imgur.com/nBGmJHz.gif",
                "https://i.imgur.com/J0pGqA3.gif",
                "https://i.imgur.com/I5hSuVU.gif",
                "https://i.imgur.com/rTczymP.gif",
            };
            try
            {
                int index = rnd.Next(happy.Count);
                var embed = new EmbedBuilder();
                embed.WithAuthor($"{Context.User.Username} is supper happy and is living it 💃");
                embed.WithImageUrl(happy[index]);
                embed.WithFooter($"[DEBUG] : {happy[index]}");
                await Context.Channel.SendMessageAsync("", false, embed: embed.Build()).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
