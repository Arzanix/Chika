using Discord.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chika.Common;
using Discord;
using Discord.WebSocket;

namespace Chika.Modules
{
    public class Information : ChikaModule
    {
        private CommandService _service;
        public Random Rand = new Random();
        public Information(CommandService service)
        {
            this._service = service;

        }

        #region  Help Menu

      
        [Command("Help")]
        [Alias("sendhelp")]
        [Summary("Sends a dm with help page")]
        public async Task Help()
        {
            var guilds = (Context.Client as DiscordSocketClient).Guilds;
            var embed = new EmbedBuilder();
            //    embed.WithAuthor(x => { x.Name = "Help Menu"; x.IconUrl = Context.Client.CurrentUser.GetAvatarUrl(); });
            //embed.WithColor(new Color(Rand.Next(0, 256), Rand.Next(0, 256), Rand.Next(0, 256)));
            //embed.WithAuthor("📧 | Message Sent !");
            //embed.WithColor(new Color(Rand.Next(0, 256), Rand.Next(0, 256), Rand.Next(0, 256)));
            //embed.WithDescription($"Hey {Context.User.Username} i've sent you a *dm* with my command menu.");
            //await Context.Channel.SendMessageAsync("", embed: embed.Build());
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            var contextString = Context.Guild?.Name ?? "DMs with me";

            var builder = new EmbedBuilder()
            {
                Title = $"{Context.Client.CurrentUser.Username}'s Help Menu",
                Description = $" Here are a list of my available commands. to find out more about any commands then `",
                // ImageUrl = Context.Client.CurrentUser.GetAvatarUrl(),
                Color = new Color(Rand.Next(0, 256), Rand.Next(0, 256), Rand.Next(0, 256)),

                ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl()
            };
            builder.WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl());
            builder.WithFooter($"〘 { _service.Commands.Count()} | Commands 〙 ● 〘  {_service.Modules.Count()} | Modules 〙 ● 〘  {guilds.Count.ToString()} | Servers 〙");

            foreach (var module in _service.Modules)
            {
                await AddModuleEmbedField(module, builder);
            }

            await Context.Channel.SendMessageAsync("", false, builder.Build());

            // Embed are limited to 24 Fields at max. So lets clear some stuff
            // out and then send it in multiple embeds if it is too big.
            builder.WithTitle("")
                .WithDescription("")
                .WithAuthor("");
            while (builder.Fields.Count > 24)
            {
                builder.Fields.RemoveRange(0, 25);
                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }
        }
        #endregion

        #region Server Statistics / Info
        [Command("serverinfo")]
        [Alias("sinfo", "serverstats", "guildinfo")]
        public async Task ServerInfoAsync(string guildName = null)
        {
            //Context.Guild doesn't case guild.users properties so here is a way around it
            var guild = (Context.Guild as SocketGuild);
            var roles = guild.Roles;
            var users = guild.Users;
            int userCount = users.Count(x => x.IsBot == false);
            var gusersnobot = users.Where(x => x.IsBot == false);
            var gusersbotonly = users.Where(x => x.IsBot == true);
            var owner = guild.Owner;
            string afkname = null;
            if (guild.AFKChannel != null)
            {
                afkname = guild.AFKChannel.Name;
            }

            var serverInfo = new EmbedBuilder()
            {
                Color = new Color(Rand.Next(0, 256), Rand.Next(0, 256), Rand.Next(0, 256)),
                ThumbnailUrl = $"{guild.IconUrl}",
                Title = $"{guild.Name}",
                Description = $"",
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"Created On: {guild.CreatedAt.UtcDateTime:dd'/'MM'/'yyyy hh:mm:ss tt} | #{guild.Id}",
                    IconUrl = $"{guild.IconUrl}"
                }
            };
            serverInfo.AddField((x) =>
            {
                x.Name = "Owner";
                x.IsInline = true;
                x.Value = $"{guild.Owner}";
            });
            serverInfo.AddField((x) =>
            {
                x.Name = "Region";
                x.IsInline = true;
                x.Value = $"{guild.VoiceRegionId}";
            });
            serverInfo.AddField((x) =>
            {
                x.Name = "Verification";
                x.IsInline = true;
                x.Value = $"{guild.VerificationLevel}";
            });
            serverInfo.AddField((x) =>
            {
                x.Name = "Members";
                x.IsInline = true;
                x.Value = $"{guild.MemberCount.ToString()}";
            });
            serverInfo.AddField((x) =>
            {
                x.Name = "Bots";
                x.IsInline = true;
                x.Value = $"{gusersbotonly.Count()}";
            });

            serverInfo.AddField((x) =>
            {
                x.Name = "Emojis";
                x.IsInline = true;
                x.Value = $"{guild.Emotes.Count}";
            });
            serverInfo.AddField((x) =>
            {
                int seconds = guild.AFKTimeout;
                string minutes = ((seconds % 3600) / 60).ToString();
                x.Name = "AFK Timeout";
                x.IsInline = true;
                x.Value = $"{minutes} minutes";
            });
            serverInfo.AddField((x) =>
            {
                x.Name = "AFK Channels";
                x.IsInline = true;
                x.Value = afkname ?? "None set";
            });

            serverInfo.AddField((x) =>
            {
                var txtchannel = guild.TextChannels.Count();
                var voicechannel = guild.VoiceChannels.Count();

                x.Name = $"Channels [{txtchannel + voicechannel}]";
                x.IsInline = true;
                x.Value = $"Categories:{ guild.CategoryChannels.Count}" + Environment.NewLine + $"Text: {txtchannel}" + Environment.NewLine + $"Voice: {voicechannel}" + Environment.NewLine + $"{afkname}" ?? "None set";
            });

            serverInfo.AddField((x) =>
            {
                x.Name = "Roles";
                x.IsInline = true;
                x.Value = $"{guild.Roles.Count - 1}";
            });

            await Context.Channel.SendMessageAsync("", embed: serverInfo.Build());
        }
        #endregion

        #region Userinfo

        [Command("userinfo")]
        [Alias("uinfo", "whois")]
        public async Task UserInfo(SocketUser user = null)
        {
            try
            {
                var userInfo = user ?? Context.User; // ?? if not null return left. if null return right
                var avatarURL = userInfo.GetAvatarUrl();
                var eb = new EmbedBuilder()
                {
                    Color = new Color(Rand.Next(0, 256), Rand.Next(0, 256), Rand.Next(0, 256)),
                    ThumbnailUrl = (avatarURL),

                    Title = $"{userInfo.Username}#{userInfo.Discriminator}",
                    Description = $"",
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = $"{userInfo.Username} ID: {userInfo.Id}",
                        IconUrl = (Context.User.GetAvatarUrl())
                    }
                };
                var socketUser = userInfo as SocketGuildUser;
                eb.AddField((x) =>
                {
                    x.Name = "Status";
                    x.IsInline = true;
                    x.Value = userInfo.Status.ToString();
                });

                eb.AddField((x) =>
                {
                    x.Name = "Playing";
                    x.IsInline = true;
                    if (userInfo.Activity == null)
                        x.Value = "None";
                    else
                    {
                        x.Value = userInfo.Activity.ToString();
                    }
                });

                eb.AddField((x) =>
                {
                    x.Name = "Nickname";
                    x.IsInline = true;
                    x.Value = $"{(socketUser.Nickname == null ? "*none*" : $"{socketUser.Nickname}")}";
                });

                eb.AddField((x) =>
                {
                    x.Name = "Discriminator";
                    x.IsInline = true;
                    x.Value = $"#{socketUser.Discriminator}";
                });

                eb.AddField((x) =>
                {
                    x.Name = "Avatar";
                    x.IsInline = true;
                    x.Value = $"[Click to View]({avatarURL})";
                });

                eb.AddField((x) =>
                {
                    x.Name = "Joined Guild";
                    x.IsInline = true;
                    x.Value = $"{socketUser?.JoinedAt.ToString().Remove(socketUser.JoinedAt.ToString().Length - 6)}\n({(int)DateTime.Now.Subtract(((DateTimeOffset)socketUser?.JoinedAt).DateTime).TotalDays} days ago)";
                });

                eb.AddField((x) =>
                {
                    string roles = "";
                    foreach (var role in socketUser.Roles)
                    {
                        if (role.Name != "@everyone")
                            roles += $"{role.Name}, ";
                    }
                    x.Name = "Roles";
                    x.IsInline = true;
                    x.Value = $"{(String.IsNullOrWhiteSpace(roles) ? "*none*" : $"{roles}")}";
                });
                eb.AddField((x) =>
                {
                    x.Name = $"Registered";
                    x.IsInline = true;
                    x.Value = $"{userInfo.CreatedAt.ToString().Remove(userInfo.CreatedAt.ToString().Length - 6)}. That is {(int)(DateTime.Now.Subtract(userInfo.CreatedAt.DateTime).TotalDays)} days ago!";
                });

                string permissions = "";
                (userInfo as SocketGuildUser)?.GuildPermissions.ToList().ForEach(x => { permissions += x.ToString() + " , "; });
                eb.AddField((x) =>
                {
                    x.Name = "Key Permissions";
                    x.IsInline = true;
                    x.Value = $"{permissions}";
                });

                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion


        #region Help Task
        private async Task AddModuleEmbedField(ModuleInfo module, EmbedBuilder builder)
        {
            if (module == null) return;
            var descriptionBuilder = new List<string>();
            var duplicateChecker = new List<string>();
            foreach (var cmd in module.Commands)
            {
                var result = await cmd.CheckPreconditionsAsync(Context);
                if (!result.IsSuccess || duplicateChecker.Contains(cmd.Aliases.First())) continue;
                duplicateChecker.Add(cmd.Aliases.First());
                var cmdDescription = $"**{cmd.Aliases.First()}**";
                if (string.IsNullOrEmpty(cmd.Summary) == false)
                    cmdDescription += $"";
                if (string.IsNullOrEmpty(cmd.Remarks) == false)
                    cmdDescription += $"";
                if (cmdDescription != "****")
                    descriptionBuilder.Add(cmdDescription);
            }

            if (descriptionBuilder.Count <= 0) return;

            var moduleNotes = "";
            if (string.IsNullOrEmpty(module.Remarks) == false)
                moduleNotes += $" {module.Remarks}";
            if (string.IsNullOrEmpty(moduleNotes) == false)
                moduleNotes += "\n";
            if (string.IsNullOrEmpty(module.Name) == false)
            {
                builder.AddField($"› **{module.Name}:**",
                    $"{moduleNotes}" + string.Join(" , ", descriptionBuilder.ToList()));
            }
        }
      
        private static Embed HelpCommand(SearchResult search, EmbedBuilder builder)
        {
            foreach (var match in search.Commands)
            {
                var cmd = match.Command;
                var parameters = cmd.Parameters.Select(p => string.IsNullOrEmpty(p.Summary) ? p.Name : p.Summary);
                var paramsString = $"Parameters: {string.Join(", ", parameters)}" +
                                   (string.IsNullOrEmpty(cmd.Remarks) ? "" : $"\nRemarks: {cmd.Remarks}") +
                                   (string.IsNullOrEmpty(cmd.Summary) ? "" : $"\nSummary: {cmd.Summary}");

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = paramsString;
                    x.IsInline = false;
                });
            }

            return builder.Build();
        }

        private async Task<Embed> HelpModule(string moduleName, EmbedBuilder builder)
        {
            var module = _service.Modules.ToList().Find(mod =>
                string.Equals(mod.Name, moduleName, StringComparison.CurrentCultureIgnoreCase));
            await AddModuleEmbedField(module, builder);
            return builder.Build();
        }
        #endregion
    }
}
