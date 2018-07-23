using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services.Events
{
    public interface IEvent
    {
        bool IsCancelled { get; set; }
    }
}
