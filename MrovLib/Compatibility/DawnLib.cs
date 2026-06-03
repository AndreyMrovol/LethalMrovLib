using System.Collections.Generic;
using System.Linq;
using Dawn;

namespace MrovLib.Compatibility
{
	public class DawnLib(string guid, string version = null) : CompatibilityBase(guid, version)
	{
		private List<SelectableLevel> _dawnLibLevels = null;
		public List<SelectableLevel> DawnLibMoons
		{
			get
			{
				if (_dawnLibLevels is null || _dawnLibLevels.Count == 0)
				{
					_dawnLibLevels = LevelHelper
						.Levels.Where(level => !level.GetDawnInfo().HasTag(NamespacedKey.From("dawn_lib", "is_external")))
						.ToList();
					Plugin.LogDebug($"Found {_dawnLibLevels.Count} DawnLib levels");
					return _dawnLibLevels;
				}
				else
				{
					return _dawnLibLevels;
				}
			}
		}

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

		public (bool locked, bool hidden) GetLevelStatus(SelectableLevel level)
		{
			return level.GetDawnInfo().DawnPurchaseInfo.PurchasePredicate.CanPurchase() switch
			{
				TerminalPurchaseResult.HiddenPurchaseResult hiddenResult => (hiddenResult.IsFailure, true),
				TerminalPurchaseResult.FailedPurchaseResult => (true, false),
				TerminalPurchaseResult.SuccessPurchaseResult => (false, false),
				_ => (false, false)
			};
		}

		public bool IsDawnLibLevel(SelectableLevel level)
		{
			return DawnLibMoons.Contains(level);
		}
	}
}
