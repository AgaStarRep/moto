using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Motohusaria.DataLayer;
using Motohusaria.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace Motohusaria.Services.Emailing
{
    [InjectableService(typeof(EmailingService))]
    class EmailingService : IEmailingService
    {
        private readonly IMapper _mapper;
        private readonly MailDbContext _db;

        public EmailingService(MailDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task InsertAsync(Mail entity)
        {
            _db.Mails.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Mail entity)
        {
            _db.Mails.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Mail entity)
        {
            _db.Mails.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Mail>> GetAllAsync()
        {
            return await _db.Mails.ToListAsync();
        }

        public async Task<Mail> GetByIdAsync(Guid id)
        {
            return await _db.Mails.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Mail> GetByIdAsync(Guid id, Func<IQueryable<Mail>, IQueryable<Mail>> includes)
        {
            throw new NotImplementedException();
        }

        public async Task<Mail> GetFirstAsync()
        {
            return await _db.Mails.FirstAsync();
        }
    }
}
