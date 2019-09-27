using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chika.Common;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Chika.Modules
{

    public class Owner : ChikaModule
    {
        public Random rnd = new Random();

        [Command("avatar"), Remarks("Sets the bots Avatar")]
        [RequireOwner]
        public async Task SetAvatar(string link)
        {
            var s = Context.Message.DeleteAsync();

            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(link);

                var stream = new MemoryStream(imageBytes);

                var image = new Image(stream);
                await Context.Client.CurrentUser.ModifyAsync(k => k.Avatar = image);

                var embed = new EmbedBuilder();
                embed.WithAuthor("Avatar Changed");
                embed.WithDescription("Successfully Changed Avatar");
                embed.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());
                embed.WithColor(new Color(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)));
                await ReplyAsync("", false, embed.Build());

            }
            catch (Exception e)
            {

                var embed = new EmbedBuilder();
                embed.WithAuthor("Avatar", Context.Message.Author.GetAvatarUrl());
                embed.WithDescription(e.Message);
                embed.WithColor(new Color(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)));
                await Context.Channel.SendMessageAsync("", false, embed.Build());

            }
        }
      
    }
}
