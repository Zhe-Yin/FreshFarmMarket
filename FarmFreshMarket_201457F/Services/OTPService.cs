using FarmFreshMarket_201457F.Data;
using FarmFreshMarket_201457F.Models;

namespace FarmFreshMarket_201457F.Services
{
    public class OTPService
    {

        private readonly AuthDbContext _context;

        public OTPService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<OTP?> GetOTP(string id)
        {
            return _context.OTPS.FirstOrDefault(x => x.UserId.Equals(id));
        }

        public void AddOTP(OTP password)
        {
            _context.OTPS.Add(password);
            _context.SaveChanges();
        }


        public void UpdateOTP(OTP password)
        {
            _context.OTPS.Update(password);
            _context.SaveChanges();
        }

        public void DeleteOTP(OTP password)
        {
            _context.OTPS.Remove(password);
            _context.SaveChanges();
        }
    }
}
