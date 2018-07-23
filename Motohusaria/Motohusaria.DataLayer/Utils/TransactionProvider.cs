
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DataLayer.Utils
{
    public class TransactionProvider : ITransactionProvider
    {
        private readonly MotohusariaDbContext _dataContext;

        public TransactionProvider(MotohusariaDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dataContext.Database.BeginTransaction();
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return _dataContext.Database.CurrentTransaction;
        }
    }
}
