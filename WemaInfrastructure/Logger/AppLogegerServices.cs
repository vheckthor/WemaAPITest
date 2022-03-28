using System;
using System.IO;
using WemaCore.Interfaces;

namespace WemaInfrastructure.Logger
{
	public class AppLoggerService : IFileLogger
	{
		static WeakReference<object> infoWeakSyncObjectReference;
		static WeakReference<object> warningWeakSyncObjectReference;
		static WeakReference<object> errorWeakSyncObjectReference;


		static object InfoSyncObj
		{
			get
			{
				if (infoWeakSyncObjectReference != null && infoWeakSyncObjectReference.TryGetTarget(out object syncObj))
				{
					return syncObj;
				}

				infoWeakSyncObjectReference = new WeakReference<object>(new object(), false);
				infoWeakSyncObjectReference.TryGetTarget(out syncObj);

				return syncObj;
			}
		}

		static object WarningSyncObj
		{
			get
			{
				if (warningWeakSyncObjectReference != null && warningWeakSyncObjectReference.TryGetTarget(out object syncObj))
				{
					return syncObj;
				}

				warningWeakSyncObjectReference = new WeakReference<object>(new object(), false);
				warningWeakSyncObjectReference.TryGetTarget(out syncObj);

				return syncObj;
			}
		}

		static object ErrorSyncObj
		{
			get
			{
				if (errorWeakSyncObjectReference != null && errorWeakSyncObjectReference.TryGetTarget(out object syncObj))
				{
					return syncObj;
				}

				errorWeakSyncObjectReference = new WeakReference<object>(new object(), false);
				errorWeakSyncObjectReference.TryGetTarget(out syncObj);

				return syncObj;
			}
		}


		public void LogError(string message, string filename, string logDirectory = null)
		{

			string LogDir = Path.Combine(Directory.GetCurrentDirectory(), "AppLog", "Error", DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MMM"), DateTime.Now.ToString("ddMMMyyy"));

			Directory.CreateDirectory(LogDir);

			string logFileName = $"{filename}_{DateTime.Today.ToString("ddMMMyyyy")}.txt";

			lock (ErrorSyncObj)
			{
				WriteLogToFile(message, Path.Combine(LogDir, logFileName));
			}
		}

		public void LogInfo(string message, string filename, string logDirectory = null)
		{
			string LogDir = Path.Combine(Directory.GetCurrentDirectory(), "AppLog", "Info", DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MMM"), DateTime.Now.ToString("ddMMMyyy"));

			Directory.CreateDirectory(LogDir);

			string logFileName = $"{filename}_{DateTime.Today.ToString("ddMMMyyyy")}.txt";

			lock (InfoSyncObj)
			{
				WriteLogToFile(message, Path.Combine(LogDir, logFileName));
			}
		}

		public void LogWarning(string message, string filename, string logDirectory = null)
		{
			string LogDir = Path.Combine(Directory.GetCurrentDirectory(), "AppLog", "Warning", DateTime.Now.ToString("yyyy"),
					DateTime.Now.ToString("MMM"), DateTime.Now.ToString("ddMMMyyy"));

			Directory.CreateDirectory(LogDir);

			string logFileName = $"{filename}_{DateTime.Today.ToString("ddMMMyyyy")}.txt";

			lock (WarningSyncObj)
			{
				WriteLogToFile(message, Path.Combine(LogDir, logFileName));
			}
		}


		public static void WriteLogToFile(string message, string logFilePath)
		{
			File.AppendAllText(logFilePath, $"------------------------------------------------------------------------------------\r\n");
			File.AppendAllText(logFilePath, $"Event Time: {DateTime.Now:hh':'mm':'ss} | {message}\r\n\r\n");
			File.AppendAllText(logFilePath, $"------------------------------------------------------------------------------------\r\n\r\n");
		}
	}
}
