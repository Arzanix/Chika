
using Chika.Configs;
using Chika.Handlers;
using Chika.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
namespace Chika
{
    class Program
    {
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        #region Private fields
        private DiscordSocketClient _client;
        private UnpunishService _unpunish;
        private ModerationService _mod;
        private CommandHandler _commands;
        private ChikaConfig _config;
        #endregion

        public async Task Start()
        {
            Console.Title = "Chika";

            // Set up our Discord client
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 100
            });

            _client.Log += Log;

            // Set up the config directory and core config
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Configurations")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Configurations"));

            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Configurations", "Chika.json")))
                _config = await ChikaConfig.UseCurrentAsync();
            else
                _config = await ChikaConfig.CreateNewAsync();

            // Set up services
            _mod = new ModerationService();
            await _mod.LoadConfigurationAsync();



            // Instantiate the dependency map and add our services and client to it.
            var serviceProvider = ConfigureServices();

            // Set up command handler
            _commands = new CommandHandler(serviceProvider);
            await _commands.InstallAsync();


            // Connect to Discord
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();



            // Hang indefinitely
            await Task.Delay(-1);
        }

        private Task Log(LogMessage m)
        {
            switch (m.Severity)
            {
                case LogSeverity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogSeverity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogSeverity.Critical: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case LogSeverity.Verbose: Console.ForegroundColor = ConsoleColor.White; break;
            }

            Console.WriteLine(m.ToString());
            if (m.Exception != null)
                Console.WriteLine(m.Exception);
            Console.ForegroundColor = ConsoleColor.Gray;

            return Task.CompletedTask;
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_client);
            services.AddSingleton(_config);
            services.AddSingleton(new CommandService());

            return new DefaultServiceProviderFactory().CreateServiceProvider(services);
        }
    }
}
