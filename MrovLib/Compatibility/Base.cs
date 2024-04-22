using System.Reflection;
using BepInEx.Bootstrap;

namespace MrovLib.Compatibility
{
  public class CompatibilityBase
  {
    internal string ModGUID { get; set; }
    internal string ModVersion { get; set; }

    private bool? _enabled;

    // constructor
    public CompatibilityBase(string guid, string version = null)
    {
      ModGUID = guid;
      ModVersion = version;
      _enabled = null;

      Plugin.logger.LogWarning($"CompatibilityBase Constructor called, GUID: {ModGUID}, Version: {ModVersion}");
    }

    public bool IsModPresent
    {
      get
      {
        Plugin.logger.LogDebug($"IsModPresent called, GUID: {ModGUID}, Enabled: {_enabled}, Version: {ModVersion}");

        if (_enabled == null)
        {
          _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ModGUID);
        }

        if (ModVersion != null && (bool)_enabled)
        {
          _enabled = Chainloader.PluginInfos[ModGUID].Metadata.Version.ToString() == ModVersion;
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
