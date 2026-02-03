using System.Collections.Generic;
using System.Linq;
using Dawn;

namespace MrovLib.Compatibility
{
	public class DawnLib(string guid, string version = null) : CompatibilityBase(guid, version)
	{
		public List<SelectableLevel> GetLevelsFromTags(string inputTag)
		{
			return MrovLib
				.LevelHelper.Levels.Where(level =>
					level
						.GetDawnInfo()
						.AllTags()
						.Any(tag => tag.Key.ToLower().Equals(inputTag.ToLower(), System.StringComparison.OrdinalIgnoreCase))
				)
				.ToList();
		}
	}
}
