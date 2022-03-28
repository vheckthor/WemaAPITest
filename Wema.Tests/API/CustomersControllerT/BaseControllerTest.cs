using Moq;
using WemaAPI.Controllers;
using WemaCore.Interfaces;
using WemaInfrastructure.Helpers.Interfaces;
using WemaInfrastructure.Repository.CustomerRepository;

namespace Wema.Tests.API.CustomersControllerT
{
	public class BaseControllerTest
	{
		protected Mock<IFileLogger> mockLogger;
		protected Mock<ICustomerDataRepository> mockCustomerRepository;
		protected Mock<ICheckLGAToStateMapping> mockMapping;
		protected CustomersController customersController;
		protected BaseControllerTest()
		{
			mockCustomerRepository = new Mock<ICustomerDataRepository>();
			mockLogger = new Mock<IFileLogger>();
			mockMapping = new Mock<ICheckLGAToStateMapping>();
			customersController = new CustomersController(mockLogger.Object, mockCustomerRepository.Object, mockMapping.Object);
		}
	}
}
