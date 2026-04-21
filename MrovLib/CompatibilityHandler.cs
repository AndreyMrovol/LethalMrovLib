using System;
using System.Reflection;
using BepInEx.Bootstrap;

namespace MrovLib
{
	public class CompatibilityHandler
	{
		public string ModGUID { get; internal set; }
		public string ModVersion { get; internal set; }

		private bool MatchExactVersion { get; set; } = true;

		private bool? _enabled;

		// constructor
		public CompatibilityHandler(string guid, string version = null, bool matchExactVersion = false)
		{
			ModGUID = guid;
			ModVersion = version;
			_enabled = null;

			MatchExactVersion = matchExactVersion;

			Plugin.DebugLogger.LogInfo(
				$"CompatibilityHandler Constructor called, GUID: {ModGUID}, Version: {ModVersion} (exact? {matchExactVersion})"
			);

			MrovLib.EventManager.MainMenuLoaded.AddListener(this.CheckIfModIsPresentAndInit);
		}

		internal void CheckIfModIsPresentAndInit()
		{
			if (!IsModPresent)
			{
				Plugin.DebugLogger.LogWarning($"Mod {ModGUID} is not present. Skipping initialization logic.");
				return;
			}

			this.Init();
		}

		public virtual void Init()
		{
			// override in derived classes for custom initialization logic
			Plugin.DebugLogger.LogInfo($"Mod {ModGUID} is present!");
		}

		public void SetToMatchExactVersion(bool setting)
		{
			MatchExactVersion = setting;
			// reset enabled to force recheck with new setting
			_enabled = null;
			Plugin.DebugLogger.LogInfo($"SetToMatchExactVersion called, setting: {setting}");
		}

		public bool IsModPresent
		{
			get
			{
				Plugin.LogDebug($"IsModPresent called, GUID: {ModGUID}, Enabled: {_enabled}, Version: {ModVersion}");

				if (_enabled == null)
				{
					_enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModGUID);
					Plugin.DebugLogger.LogInfo($"Mod presence checked: {_enabled}");
				}

				if (ModVersion != null && (bool)_enabled)
				{
					// compare semver to match at least provided version

					if (Chainloader.PluginInfos.TryGetValue(ModGUID, out BepInEx.PluginInfo pluginInfo))
					{
						Plugin.LogDebug($"Checking version {pluginInfo.Metadata.Version} against {ModVersion}");
						// make sure the biggest version (X.0.0.0) matches

						if (pluginInfo.Metadata.Version.Major != new Version(ModVersion).Major)
						{
							_enabled = false;
						}
						else
						{
							if (MatchExactVersion)
							{
								_enabled = pluginInfo.Metadata.Version == new Version(ModVersion);
							}
							else
							{
								_enabled = pluginInfo.Metadata.Version >= new Version(ModVersion);
							}
						}
					}
				}

				return (bool)_enabled;
			}
		}

		public Assembly GetModAssembly
		{
			get
			{
				if (!IsModPresent)
				{
					return null;
				}

				return Chainloader.PluginInfos[ModGUID].Instance.GetType().Assembly;
			}
		}
	}
}
