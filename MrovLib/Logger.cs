using BepInEx.Configuration;
using BepInEx.Logging;

namespace MrovLib
{
	public class Logger
	{
		private static ManualLogSource LogSource;
		private static ConfigEntry<bool> ConfigEntry;

		public Logger(string name, ConfigEntry<bool> enabled)
		{
			LogSource = BepInEx.Logging.Logger.CreateLogSource(name);
			ConfigEntry = enabled;
		}

		// this is overengineered to fuck
		public static void Log(LogLevel level, object data)
		{
			if (ConfigEntry.Value)
			{
				LogSource.Log(level, data);
			}
		}

		public void LogInfo(object data)
		{
			Log(LogLevel.Info, data);
		}

		public void LogWarning(object data)
		{
			Log(LogLevel.Warning, data);
		}

		public void LogError(object data)
		{
			Log(LogLevel.Error, data);
		}

		public void LogDebug(object data)
		{
			Log(LogLevel.Debug, data);
		}

		public void LogFatal(object data)
		{
			Log(LogLevel.Fatal, data);
		}

		public void LogMessage(object data)
		{
			Log(LogLevel.Message, data);
		}
	}
}
