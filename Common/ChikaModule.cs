using System;
using System.Collections.Generic;
using System.Text;
using Chika.Embedz;
using Chika.Handlers;
using Discord.Commands;

namespace Chika.Common
{
    public abstract class ChikaModule : ModuleBase
    {
        public CommandHandler commandHandler { get; }
        public ICommandContext ctx => Context;

      //  public Embedinator embedz{ get; set; }
    }
}
