using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MrovLib.Compatibility
{
  public class WeatherTweaks(string guid, string version = null) : CompatibilityBase(guid, version)
  {
    internal static MethodInfo GetPlanetCurrentWeather;

    public static void GetMethodType()
    {
      // Get the assembly that contains the class
      var assembly = Plugin.WeatherTweaks.GetModAssembly;

      // Get the Type object for the class
      var type = assembly.GetType($"{Plugin.WeatherTweaks.ModGUID}.Variables");

      if (type != null)
      {
        Plugin.logger.LogInfo($"Type {type} found");

        GetPlanetCurrentWeather = type.GetMethod("GetPlanetCurrentWeather", BindingFlags.Public | BindingFlags.Static);

        if (GetPlanetCurrentWeather != null)
        {
          Plugin.logger.LogInfo($"Method {GetPlanetCurrentWeather} found - BetaWeatherTweaks");
        }
        else
        {
          Plugin.logger.LogError($"Method {GetPlanetCurrentWeather} not found");

          // check if the method is internal static string GetPlanetCurrentWeather(SelectableLevel level)
          // if not, log an error

          GetPlanetCurrentWeather = type.GetMethod("GetPlanetCurrentWeather", BindingFlags.NonPublic | BindingFlags.Static);

          if (GetPlanetCurrentWeather != null)
          {
            Plugin.logger.LogInfo($"Method {GetPlanetCurrentWeather} found");
          }
          else
          {
            Plugin.logger.LogError($"Method {GetPlanetCurrentWeather} not found");
          }
        }
      }
      else
      {
        Plugin.LogDebug($"Type {Plugin.WeatherTweaks.ModGUID}.Variables not found");
      }
    }

    public static string CurrentWeather(SelectableLevel level)
    {
      // call WeatherTweaks.Variables public static string GetPlanetCurrentWeather(SelectableLevel level, bool uncertain = true) using reflection
      // return the result

      if (Plugin.WeatherTweaks.IsModPresent)
      {
        if (GetPlanetCurrentWeather == null)
        {
          GetMethodType();
        }
      }

      if (GetPlanetCurrentWeather != null)
      {
        return (string)GetPlanetCurrentWeather.Invoke(null, new object[] { level, true });
      }
      else
      {
        Plugin.logger.LogError("GetPlanetCurrentWeather method not found");
        return "";
      }
    }
  }
}
