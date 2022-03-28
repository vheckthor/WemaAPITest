using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WemaCore.Helpers
{
	public class StatesAndLGAs
	{
		public State State { get; set; }
	}

	public class Local
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("id")]
		public int Id { get; set; }
	}

	public class State
	{
		[JsonPropertyName("capital")]
		public string Capital { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("locals")]
		public List<Local> Locals { get; set; }

		public State()
		{
			Locals = new List<Local>();
		}
	}
}
