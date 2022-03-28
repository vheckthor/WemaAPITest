using System;
using System.Collections.Generic;

namespace WemaCore.Entities
{
	public class ListOfBanks
	{
		public List<Result> Result { get; set; }
		public object ErrorMessage { get; set; }
		public object ErrorMessages { get; set; }
		public bool HasError { get; set; }
		public DateTime TimeGenerated { get; set; }
	}
	public class Result
	{
		public string BankName { get; set; }
		public string BankCode { get; set; }
	}
}
