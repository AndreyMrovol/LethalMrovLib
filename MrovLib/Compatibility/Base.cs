using System;
using System.Reflection;
using BepInEx.Bootstrap;

namespace MrovLib.Compatibility
{
	public class CompatibilityBase
	{
		public string ModGUID { get; internal set; }
		public string ModVersion { get; internal set; }

		private bool? _enabled;

		// constructor
		public CompatibilityBase(string guid, string version = null)
		{
			ModGUID = guid;
			ModVersion = version;
			_enabled = null;

			Plugin.DebugLogger.LogWarning($"CompatibilityBase Constructor called, GUID: {ModGUID}, Version: {ModVersion}");
		}

		public bool IsModPresent
		{
			get
			{
				// Plugin.LogDebug($"IsModPresent called, GUID: {ModGUID}, Enabled: {_enabled}, Version: {ModVersion}");

				if (_enabled == null)
				{
					_enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModGUID);
				}

				if (ModVersion != null && (bool)_enabled)
				{
					// compare semver to match at least provided version

					if (Chainloader.PluginInfos.TryGetValue(ModGUID, out BepInEx.PluginInfo pluginInfo))
					{
						// Plugin.LogDebug($"Checking version {pluginInfo.Metadata.Version} against {ModVersion}");
						// make sure the biggest version (X.0.0.0) matches

						if (pluginInfo.Metadata.Version.Major != new Version(ModVersion).Major)
						{
							_enabled = false;
						}
						else
						{
							_enabled = pluginInfo.Metadata.Version >= new Version(ModVersion);
						}
					}
				}

				// Plugin.LogDebug($"Returning {_enabled} ({(bool)_enabled})");
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
