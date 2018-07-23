using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DataLayer.Utils
{
    public interface ITransactionProvider
{
    IDbContextTransaction BeginTransaction();

    IDbContextTransaction GetCurrentTransaction();
}
}
