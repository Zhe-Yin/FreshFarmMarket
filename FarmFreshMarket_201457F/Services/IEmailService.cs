using FarmFreshMarket_201457F.Models;

namespace FarmFreshMarket_201457F.Services
{
	public interface IEmailService
	{
		Task SendEmailForOTP(UserEmailOptions userEmailOptions);

		Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
	}
}
