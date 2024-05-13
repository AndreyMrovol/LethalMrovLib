using System.Collections.Generic;
using System.Linq;
using MrovLib.Compatibility;
using UnityEngine;

namespace MrovLib.API
{
  public class SharedMethods
  {
    public static string GetWeather(SelectableLevel level)
    {
      string weather;

      if (Plugin.WeatherTweaks.IsModPresent)
      {
        weather = WeatherTweaks.CurrentWeather(level);
      }
      else if (Plugin.LLL.IsModPresent)
      {
        weather = LLL.GetWeather(level);
      }
      else
      {
        weather = level.currentWeather.ToString();
      }

      return weather == "None" ? "" : weather;
    }

    public static string GetNumberlessPlanetName(SelectableLevel level)
    {
      return new string(level.PlanetName.SkipWhile(c => !char.IsLetter(c)).ToArray());
    }

    public static List<GrabbableObject> GetShipObjects()
    {
      GameObject ship = GameObject.Find("/Environment/HangarShip");
      return ship.GetComponentsInChildren<GrabbableObject>().ToList();
    }

    public static List<SelectableLevel> GetGameLevels()
    {
      Plugin.logger.LogWarning("GetGameLevels called");

      if (Plugin.LLL.IsModPresent)
      {
        Plugin.LogDebug($"LLL present");
        return LLL.GetLevels();
      }
      else if (LLLOldPlugin.IsTheOldLLLActive())
      {
        Plugin.LogDebug($"LLLOld present");
        return LLLOldPlugin.GetSelectableLevels();
      }
      else
      {
        Plugin.LogDebug($"NOLLL present");
        return StartOfRound.Instance.levels.ToList();
      }
    }

    public static bool IsMoonHiddenLLL(SelectableLevel level)
    {
      if (Plugin.LLL.IsModPresent)
      {
        return LLL.IsMoonHidden(level);
      }
      else if (LLLOldPlugin.IsTheOldLLLActive())
      {
        return LLLOldPlugin.IsMoonHidden(level);
      }

      return false;
    }

    public static bool IsMoonLockedLLL(SelectableLevel level)
    {
      if (Plugin.LLL.IsModPresent)
      {
        return LLL.IsMooonLocked(level);
      }
      else if (LLLOldPlugin.IsTheOldLLLActive())
      {
        return LLLOldPlugin.IsMoonLocked(level);
      }

      return false;
    }

    public static List<TerminalNode> GetLevelTerminalNodes(SelectableLevel level)
    {
      if (Plugin.LLL.IsModPresent)
      {
        return LLL.GetLevelTerminalNodes(level);
      }
      else
      {
        return [];
      }
    }

    public static object GetLLLMoonsCataloguePage()
    {
      if (Plugin.LLL.IsModPresent)
      {
        return LLL.GetMoonsCataloguePage();
      }
      else if (LLLOldPlugin.IsTheOldLLLActive())
      {
        return LLLOldPlugin.GetMoonsCataloguePage();
      }

      return null;
    }

    // public static int GetPrice(int beforeDiscountPrice)
    // {
    //     Plugin.logger.LogWarning($"price: {beforeDiscountPrice}");

    //     if (Plugin.isLGUPresent)
    //     {
    //         Plugin.logger.LogInfo($"LGU is present");
    //         return LategameUpgradesCompatibility.GetMoonPrice(beforeDiscountPrice);
    //     }
    //     else
    //     {
    //         return beforeDiscountPrice;
    //     }
    // }
  }
}
