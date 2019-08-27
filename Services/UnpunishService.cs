using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Chika.Configs;
using Chika.Data;

namespace Chika.Services
{
    public class UnpunishService
    {
        private DiscordSocketClient _client;

        private ChikaConfig _config;
        private bool _running;



        public async Task StartAsync()
        {
            _running = true;
            while (_running)
            {
                await Task.Delay(3000);
               
              
            }
        }
    }
}
