using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services.Logger
{
    public interface ILoggerDataProvider
    {
        KeyValuePair<string, object>[] GetData();
    }
}
