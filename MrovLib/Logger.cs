using System;
using System.Linq;
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
		private string _name;
		public virtual string ModName => "MrovLib";
		private ManualLogSource _logSource = BepInEx.Logging.Logger.CreateLogSource($"MrovLib");

		private LoggingType _defaultLoggingType;

		public Logger(string SourceName, LoggingType defaultLoggingType = LoggingType.Basic)
		{
			_defaultLoggingType = defaultLoggingType;
			_name = SourceName == ModName ? "" : SourceName;
		}

		[Obsolete("Use Logger(string SourceName, LoggingType defaultLoggingType) instead.")]
		public Logger(string name)
			: this(name, LoggingType.Debug) { }

		[Obsolete("Use Logger(string SourceName, LoggingType defaultLoggingType) instead.")]
		public Logger(string name, ConfigEntry<bool> enabled = null)
			: this(name, LoggingType.Debug) { }

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

		#region Obsolete definitions

		[Obsolete("Use LogInfo(string data) instead.")]
		public void LogInfo(object data)
		{
			Log(LogLevel.Info, data.ToString());
		}

		[Obsolete("Use LogWarning(string data) instead.")]
		public void LogWarning(object data)
		{
			Log(LogLevel.Warning, data.ToString());
		}

		[Obsolete("Use LogError(string data) instead.")]
		public void LogError(object data)
		{
			Log(LogLevel.Error, data.ToString());
		}

		[Obsolete("Use LogDebug(string data) instead.")]
		public void LogDebug(object data)
		{
			Log(LogLevel.Debug, data.ToString());
		}

		[Obsolete("Use LogFatal(string data) instead.")]
		public void LogFatal(object data)
		{
			Log(LogLevel.Fatal, data.ToString());
		}

		[Obsolete("Use LogMessage(string data) instead.")]
		public void LogMessage(object data)
		{
			Log(LogLevel.Message, data.ToString());
		}
	}
}
		#endregion
