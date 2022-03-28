using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WemaCore.Constants;
using WemaCore.DTOs.ApiResponse;
using WemaInfrastructure.ExternalEndpoints;

namespace WemaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GetAllBanksController : ControllerBase
	{
		private readonly IGetListofBanks _listOfBanks;

		public GetAllBanksController(IGetListofBanks listOfBanks)
		{
			_listOfBanks = listOfBanks;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var response = await _listOfBanks.GetBankListAsync();
			if (response != null)
			{
				return Ok(new BankListResponse{ BankList = response.Result });
			}
			return NotFound(AppConstants.DATANOTFOUND);
		}
	}
}
