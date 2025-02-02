﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Chika.Configs;
using Chika.Modules;

namespace Chika.Handlers
{
    public class CommandHandler
    {
        private IServiceProvider _services;
        private DiscordSocketClient _client;
        private CommandService _commands;
        private ChikaConfig _config;

        public CommandHandler(IServiceProvider services)
        {
            _client = services.GetService<DiscordSocketClient>();
            _config = services.GetService<ChikaConfig>();
            _commands = services.GetService<CommandService>();
            _services = services;
        }

        public async Task InstallAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task HandleCommandAsync(SocketMessage m)
        {
            var message = m as SocketUserMessage;
            if (message == null) return;
            if (!(message.Channel is SocketGuildChannel)) return;
            if (m.Author.Id == _client.CurrentUser.Id) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with the bot's prefix or a mention prefix
            if (message.HasStringPrefix(_config.Prefix, ref argPos) || _config.MentionPrefixEnabled(message, _client, ref argPos))
            {
                // Create a Command Context
                var context = new CommandContext(_client, message);
                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed succesfully)
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    switch (result)
                    {
                        case ExecuteResult exResult:
                            Console.WriteLine(exResult.Exception);
                            break;
                        case PreconditionResult preResult:
                            await (await m.Author.GetOrCreateDMChannelAsync()).SendMessageAsync($":x: {preResult.ErrorReason}");
                            break;
                        default:
                            await (await m.Author.GetOrCreateDMChannelAsync()).SendMessageAsync(result.ErrorReason);
                            break;
                    }
                }
            }
        }
    }
}
