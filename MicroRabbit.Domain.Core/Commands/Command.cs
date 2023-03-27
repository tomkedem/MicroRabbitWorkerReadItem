using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.Core.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestemp { get; protected set; } 
        protected Command()
        {
            Timestemp = DateTime.Now;
        }
    }
}
