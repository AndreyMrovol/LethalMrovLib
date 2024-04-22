using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using LethalLevelLoader;

namespace MrovLib.Compatibility
{
  public class LLL(string guid, string version = null) : CompatibilityBase(guid, version)
  {
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static string GetWeather(SelectableLevel level)
    {
      // get ExtendedLevel from SelectableLevel
      ExtendedLevel extendedLevel = LethalLevelLoader.PatchedContent.ExtendedLevels.FirstOrDefault(x => x.selectableLevel == level);

      // use reflection to call TerminalManager.GetWeatherConditions - must invoke the original method cause of weathertweaks
      // it's internal static method
      var weatherCondition = LethalLevelLoader.TerminalManager.GetWeatherConditions(extendedLevel).ToString().Replace("(", "").Replace(")", "");

      return weatherCondition;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static List<SelectableLevel> GetLevels()
    {
      return LethalLevelLoader.PatchedContent.SeletectableLevels;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool IsMoonHidden(SelectableLevel level)
    {
      ExtendedLevel extendedLevel = LethalLevelLoader.PatchedContent.ExtendedLevels.FirstOrDefault(x => x.selectableLevel == level);
      return extendedLevel.IsRouteHidden;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static MoonsCataloguePage GetMoonsCataloguePage()
    {
      return LethalLevelLoader.TerminalManager.currentMoonsCataloguePage;
    }
  }
}
