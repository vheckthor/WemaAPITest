using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WemaAPI.Controllers;
using WemaInfrastructure.ExternalEndpoints;

namespace Wema.Tests.API.GetAllBanksControllerT
{
	public class BaseControllerTest
	{
		protected Mock<IGetListofBanks> mockBankList;
		protected GetAllBanksController getAllBanksController;
		protected BaseControllerTest()
		{
			mockBankList = new Mock<IGetListofBanks>();
			getAllBanksController = new GetAllBanksController(mockBankList.Object);
		}
	}
}
