using System;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace MrovLib
{
	public enum LoggingType
	{
		Basic,
		Debug,
		Developer,
	}

	public class Logger
	{
		public virtual string ModName => "MrovLib";
		private string _name;
		private ManualLogSource _logSource = BepInEx.Logging.Logger.CreateLogSource($"MrovLib");

		private LoggingType _defaultLoggingType;

		public Logger(string SourceName, LoggingType defaultLoggingType = LoggingType.Basic)
		{
			_defaultLoggingType = defaultLoggingType;
			_name = SourceName == ModName ? "" : SourceName;
		}

		[Obsolete("Use Logger(string SourceName, LoggingType defaultLoggingType) instead.")]
		public Logger(string name)
		{
			_defaultLoggingType = LoggingType.Debug;
			_name = name == ModName ? "" : name;
		}

		[Obsolete("Use Logger(string SourceName, LoggingType defaultLoggingType) instead.")]
		public Logger(string name, ConfigEntry<bool> enabled = null)
		{
			_defaultLoggingType = LoggingType.Developer;
			_name = name == ModName ? "" : name;
		}

		[Obsolete("Use Logger(string SourceName, LoggingType defaultLoggingType) instead.")]
		public Logger(string name, ConfigEntry<LoggingType> defaultType)
		{
			_defaultLoggingType = defaultType.Value;
			_name = name == ModName ? "" : name;
		}

		public virtual bool ShouldLog(LoggingType type)
		{
			return LocalConfigManager.Debug.Value >= type;
		}

		public void LogCustom(string data, LogLevel level, LoggingType type)
		{
			if (ShouldLog(type))
			{
				_logSource.Log(level, $"[{_name}] {data}");
			}
		}

		public void Log(LogLevel level, string data)
		{
			if (ShouldLog(_defaultLoggingType))
			{
				_logSource.Log(level, $"[{_name}] {data}");
			}
		}

		public void LogInfo(string data)
		{
			Log(LogLevel.Info, data);
		}

		public void LogWarning(string data)
		{
			Log(LogLevel.Warning, data);
		}

		public void LogError(string data)
		{
			Log(LogLevel.Error, data);
		}

		public void LogDebug(string data)
		{
			Log(LogLevel.Debug, data);
		}

		public void LogFatal(string data)
		{
			Log(LogLevel.Fatal, data);
		}

		public void LogMessage(string data)
		{
			Log(LogLevel.Message, data);
		}
	}
}
