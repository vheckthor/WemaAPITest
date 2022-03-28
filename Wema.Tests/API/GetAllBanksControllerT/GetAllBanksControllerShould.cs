using Moq;
using System;
using System.Collections.Generic;
using WemaCore.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using WemaCore.DTOs.ApiResponse;
using WemaCore.Constants;

namespace Wema.Tests.API.GetAllBanksControllerT
{
	public class GetAllBanksControllerShould : BaseControllerTest
	{
		[Fact]
		public async Task Return_All_Banks_Available()
		{
			mockBankList.Setup(_ => _.GetBankListAsync())
				.ReturnsAsync(
				new ListOfBanks
				{
					ErrorMessage = null,
					ErrorMessages = null,
					HasError = false,
					Result = new List<Result>
					{
						new Result {
							BankName = "FIRST CITY MONUMENT BANK",
							BankCode = "122"
						},
						new Result  {
							BankName = "FIRSTERS",
							BankCode = "900"
						},
							new Result{
							BankName = "GUARANTY TRUST BANK",
							BankCode= "058"
						},
							new Result{
							BankName= "HERITAGE BANK",
							BankCode= "030"
						},
							new Result{
							BankName = "IMPERIAL HOMES MORTGAGE BANK",
							BankCode = "415"
						},
					},
					TimeGenerated = DateTime.Now
				});

			var response = await getAllBanksController.Get();
			Assert.NotNull(response);
			var okObjectResult = Assert.IsType<OkObjectResult>(response);
			Assert.True(okObjectResult.StatusCode == 200);
			var more = Assert.IsType<BankListResponse>(okObjectResult.Value);
			Assert.Equal(5, more.BankList.Count);
		}

		[Fact]
		public async Task Return_NOT_FOUND_WHEN_NO_Bank_IS_Available()
		{
			mockBankList.Setup(_ => _.GetBankListAsync());
			var response = await getAllBanksController.Get();
			Assert.NotNull(response);
			var objectResult = Assert.IsType<NotFoundObjectResult>(response);
			Assert.True(objectResult.StatusCode == 404);
			var more = Assert.IsType<string>(objectResult.Value);
			Assert.Equal(AppConstants.DATANOTFOUND, more);
		}

	}
}
