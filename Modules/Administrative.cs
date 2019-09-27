using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Chika.Common;
using Chika.Embedz;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Chika.Modules
{

    //TODO FML
    public class Administrative : ChikaModule
    {
        [Command("ban")]
        [RequireUserPermission(GuildPermission.Administrator | GuildPermission.BanMembers)]
        public async Task BanAsync(SocketGuildUser user, string reason = "no reason has been provided.")
        {


            if (user.Id == ChikaBot.TurkeyLoverz)
            {
                await Embedinator.Error(ctx, "Invalid Action",
                    "I am unable to ban this user due to internal protections.");
            }
            else if (user.Id == ChikaBot.Arzanix)
            {
                await Embedinator.Error(ctx, "Invalid Action",
                    "I am unable to ban this user due to internal protections.");
            }
            else
            {
                await ctx.Guild.AddBanAsync(user, pruneDays: 7, reason);
                await Embedinator.Successful(ctx, "Successfully Banned",
                    $"{user.Username} Has been banned from {Format.Bold("ctx.Guild.Name")} for the following reason {Format.Code(reason)}");
            }
        }
        [Command("kick")]
        [RequireUserPermission(GuildPermission.Administrator | GuildPermission.BanMembers | GuildPermission.KickMembers)]

        public async Task KickAsync(SocketGuildUser user, string reason = "no reason has been provided.")
        {

        }
        [Command("idban")]
        [RequireUserPermission(GuildPermission.Administrator | GuildPermission.BanMembers | GuildPermission.KickMembers)]

        public async Task HackBanAsync(SocketGuildUser user, string reason = "no reason has been provided.")
        {

        }
    }
}
