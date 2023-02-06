using FarmFreshMarket_201457F.Data;
using FarmFreshMarket_201457F.Models;

namespace FarmFreshMarket_201457F.Services
{
    public class ResetPasswordService
    {

        private readonly AuthDbContext _context;

        public ResetPasswordService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<ResetPassword?> GetPasswordReset(string id)
        {

            
            return _context.ResetPasswords.FirstOrDefault(x => x.UserId.Equals(id));
        }

        public void AddResetPassword(ResetPassword resetPassword)
        {
            _context.ResetPasswords.Add(resetPassword);
            _context.SaveChanges();
        }


        public void UpdateResetPassword(ResetPassword resetPassword)
        {
            _context.ResetPasswords.Update(resetPassword);
            _context.SaveChanges();
        }
    }
}
