using FarmFreshMarket_201457F.Data;
using FarmFreshMarket_201457F.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmFreshMarket_201457F.Services
{
    public class LogService
    {
        private readonly AuthDbContext _context;

        public LogService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<List<Log>> RetreiveAllLogs()
        {
            return await _context.Logs.OrderByDescending(log => log.CreateTime).ToListAsync();
        }

        public async Task RecordLogs(string action,string email)
        {
            await _context.Logs.AddAsync(new Log()
            {
                Action = action,
                Description = string.Format("User: {0} has logged in!", email),
                User = email
            });
        }
    }
}
