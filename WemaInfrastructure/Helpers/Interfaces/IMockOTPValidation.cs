namespace WemaInfrastructure.Helpers.Interfaces
{
	public interface IMockOTPValidation
	{
		bool ConfirmOTP(int otp, string phoneNumber);
		int SendOTP(string phoneNumber);
		int ResendOTP(string phoneNumber);
	}
}