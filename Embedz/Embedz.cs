using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Chika.Embedz
{
    public class Embedinator
    {
        public static async Task Error(ICommandContext context, string title, string msg)
        {
            var image = context.User.GetAvatarUrl(ImageFormat.Auto, 64);
            var builder = new EmbedBuilder();
            builder.WithDescription(msg);
            builder.WithAuthor(title, image);
            builder.WithColor(178, 34, 34);

            var embed = builder.Build();
            await context.Channel.SendMessageAsync("", false, embed);
        }
        public static async Task Successful(ICommandContext context, string title, string msg)
        {
            var image = context.User.GetAvatarUrl(ImageFormat.Auto, 64);
            var builder = new EmbedBuilder();
            builder.WithDescription(msg);
            builder.WithAuthor(title, image);
            builder.WithColor(0, 255, 127);

            var embed = builder.Build();
            await context.Channel.SendMessageAsync("", false, embed);
        }

    }
}
