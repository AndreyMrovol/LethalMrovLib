using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using LethalLevelLoader;
using OldLLLLib;

namespace MrovLib.Compatibility
{
  public class LLLOldPlugin(string guid, string version = null) : CompatibilityBase(guid, version)
  {
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool IsTheOldLLLActive()
    {
      return OldLLLLib.Plugin.IsOldLLLPresent;
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static string GetWeather(SelectableLevel level)
    {
      return OldLLLLib.LLL.GetWeather(level);
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static List<SelectableLevel> GetSelectableLevels()
    {
      return OldLLLLib.LLL.GetSelectableLevels();
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static bool IsMoonHidden(SelectableLevel level)
    {
      return OldLLLLib.LLL.IsMoonHidden(level);
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static MoonsCataloguePage GetMoonsCataloguePage()
    {
      return OldLLLLib.LLL.GetMoonsCataloguePage();
    }
  }
}
