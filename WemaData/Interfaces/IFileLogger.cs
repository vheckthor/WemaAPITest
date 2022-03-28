namespace WemaCore.Interfaces
{
	public interface IFileLogger
	{
		void LogError(string message, string filename, string logDirectory = null);
		void LogInfo(string message, string filename, string logDirectory = null);
		void LogWarning(string message, string filename, string logDirectory = null);
	}
}
