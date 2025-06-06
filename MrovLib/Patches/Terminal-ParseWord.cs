using HarmonyLib;

namespace MrovLib.Patches
{
	// nicked this from https://github.com/IAmBatby/LethalLevelLoader/blob/main/LethalLevelLoader/Patches/TerminalManager.cs
	// love ya Batby

	[HarmonyPatch(typeof(Terminal), "ParseWord")]
	[HarmonyAfter(LethalLevelLoader.Plugin.ModGUID)]
	internal static class TerminalParseWord
	{
		internal static TerminalKeyword lastParsedVerbKeyword;

		[HarmonyPostfix]
		internal static void TerminalParseWord_Postfix(Terminal __instance, ref TerminalKeyword __result, string playerWord)
		{
			if (__result != null)
			{
				TerminalKeyword newKeyword = TryFindAlternativeNoun(__instance, __result, playerWord);
				if (newKeyword != null)
				{
					__result = newKeyword;
				}
			}
		}

		internal static bool ValidateNounKeyword(TerminalKeyword verbKeyword, TerminalKeyword nounKeyword)
		{
			for (int k = 0; k < verbKeyword.compatibleNouns.Length; k++)
			{
				if (verbKeyword.compatibleNouns[k].noun == nounKeyword)
				{
					return true;
				}
			}
			return false;
		}

		internal static TerminalKeyword TryFindAlternativeNoun(Terminal terminal, TerminalKeyword foundKeyword, string playerInput)
		{
			if (foundKeyword != null & terminal.hasGottenVerb == false && foundKeyword.isVerb == true)
			{
				lastParsedVerbKeyword = foundKeyword;
			}

			if (foundKeyword != null && foundKeyword.isVerb == false && terminal.hasGottenVerb == true && lastParsedVerbKeyword != null)
			{
				TerminalKeyword nounKeyword = foundKeyword;
				if (ValidateNounKeyword(lastParsedVerbKeyword, nounKeyword) == false)
				{
					foreach (TerminalKeyword newNounKeyword in ContentManager.Terminal.terminalNodes.allKeywords)
					{
						if (newNounKeyword.isVerb == false && newNounKeyword != nounKeyword && newNounKeyword.word == playerInput)
						{
							if (ValidateNounKeyword(lastParsedVerbKeyword, newNounKeyword) == true)
							{
								lastParsedVerbKeyword = null;
								return newNounKeyword;
							}
						}
					}
				}
			}

			return foundKeyword;
		}
	}
}
