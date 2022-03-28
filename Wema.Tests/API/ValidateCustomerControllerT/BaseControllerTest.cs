using Moq;
using WemaAPI.Controllers;
using WemaInfrastructure.Helpers.Interfaces;
using WemaInfrastructure.Repository.CustomerRepository;

namespace Wema.Tests.API.ValidateCustomerControllerT
{
	public class BaseControllerTest
	{
		protected ValidateCustomerController validateCustomer;
		protected Mock<IMockOTPValidation> mockOTP;
		protected Mock<ICustomerDataRepository> mockCustomerRepository;
		protected BaseControllerTest()
		{
			mockOTP = new Mock<IMockOTPValidation>();
			mockCustomerRepository = new Mock<ICustomerDataRepository>();
			validateCustomer = new ValidateCustomerController(mockOTP.Object, mockCustomerRepository.Object);
		}
	}
}
