using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MrovLib.ContentType;
using UnityEngine;

namespace MrovLib
{
	public class ContentManager
	{
		internal static Terminal Terminal;
		internal static List<TerminalNode> Nodes = [];

		internal static List<TerminalKeyword> Keywords = [];
		internal static List<TerminalKeyword> Verbs => Keywords.Where(k => k.isVerb).ToList();

		internal static TerminalKeyword RouteKeyword => Verbs.Where(v => v.name == "Route").FirstOrDefault();
		internal static TerminalKeyword RouteInfoKeyword => Verbs.Where(v => v.name == "Info").FirstOrDefault();
		internal static TerminalKeyword RouteConfirmKeyword => Verbs.Where(v => v.name == "Confirm").FirstOrDefault();
		internal static TerminalKeyword RouteDenyKeyword => Verbs.Where(v => v.name == "Deny").FirstOrDefault();
		internal static TerminalKeyword MoonsKeyword => Keywords.Where(v => v.name == "Moons").FirstOrDefault();
		internal static TerminalKeyword ViewKeyword => Verbs.Where(v => v.name == "View").FirstOrDefault();
		internal static TerminalKeyword BuyKeyword => Verbs.Where(v => v.name == "Buy").FirstOrDefault();
		internal static TerminalNode CancelRouteNode => RouteKeyword?.compatibleNouns[0].result.terminalOptions[0].result;
		internal static TerminalNode CancelPurchaseNode => BuyKeyword?.compatibleNouns[0].result.terminalOptions[1].result;

		public static List<BuyableThing> Buyables = [];
		public static List<Creature> Creatures = [];

		public static List<BuyableItem> Items =>
			Buyables.Where(b => b.Type == PurchaseType.Item).Select(buyable => buyable as BuyableItem).ToList();
		public static List<BuyableDecoration> Decorations =>
			Buyables.Where(b => b.Type == PurchaseType.Decoration).Select(buyable => buyable as BuyableDecoration).ToList();
		public static List<BuyableUnlockable> Unlockables =>
			Buyables.Where(b => b.Type == PurchaseType.Unlockable).Select(buyable => buyable as BuyableUnlockable).ToList();
		public static List<BuyableSuit> Suits =>
			Buyables.Where(b => b.Type == PurchaseType.Suit).Select(buyable => buyable as BuyableSuit).ToList();

		public static List<BuyableCar> Vehicles =>
			Buyables.Where(b => b.Type == PurchaseType.Vehicle).Select(buyable => buyable as BuyableCar).ToList();
		public static List<BuyableBundle> Bundles =>
			Buyables.Where(b => b.Type == PurchaseType.Bundle).Select(buyable => buyable as BuyableBundle).ToList();

		public static List<Route> Routes = [];
		public static List<Scrap> Scraps = [];

		public static RouteDictionary RouteDictionary = new();

		public static BuyableThing GetBuyable(Item item)
		{
			return Buyables.Where(b => b.Name == item.itemName).FirstOrDefault();
		}

		public static BuyableThing GetBuyable(UnlockableItem unlockable)
		{
			return Buyables.Where(b => b.Name == unlockable.unlockableName).FirstOrDefault();
		}

		public static BuyableThing GetBuyable(BuyableVehicle vehicle)
		{
			return Buyables.Where(b => b.Name == vehicle.vehicleDisplayName).FirstOrDefault();
		}

		public static BuyableThing GetBuyable(string name)
		{
			return Buyables.Where(b => b.Name.ToLowerInvariant() == name.ToLowerInvariant()).FirstOrDefault();
		}

		public static BuyableThing GetBuyable(TerminalNode node)
		{
			return Buyables.Where(b => b.Nodes.Node == node || b.Nodes.NodeConfirm == node).FirstOrDefault();
		}

		public static void AddTerminalKeywords(List<TerminalKeyword> keywords)
		{
			List<TerminalKeyword> terminalKeywords = Terminal.terminalNodes.allKeywords.ToList();

			terminalKeywords.AddRange(keywords);
			Terminal.terminalNodes.allKeywords = terminalKeywords.ToArray();

			Keywords = Terminal.terminalNodes.allKeywords.ToList();
		}

		public static void AddTerminalNodes(List<TerminalNode> nodes)
		{
			List<TerminalNode> terminalNodes = Terminal.terminalNodes.terminalNodes.ToList();

			terminalNodes.AddRange(nodes);
			Terminal.terminalNodes.terminalNodes = terminalNodes;

			Nodes = terminalNodes;
		}

		internal static void Clear()
		{
			Nodes.Clear();
			Keywords.Clear();
			Terminal = null;

			Buyables.Clear();
			Creatures.Clear();

			Routes.Clear();
			Scraps.Clear();

			RouteDictionary.Clear();
		}

		public static void Init(Terminal terminal)
		{
			ContentManager.Clear();

			ContentManager.Terminal = terminal;

			#region Get all nodes

			List<TerminalNode> Nodes = Resources.FindObjectsOfTypeAll<TerminalNode>().ToList();

			if (!Plugin.MapperRestoreCompat.IsModPresent)
			{
				Plugin.logger.LogDebug("Forcefully removing Mapper nodes");
				Nodes.RemoveAll(node => node.name.ToLower().Contains("mapper"));
			}

			if (Plugin.ShipInventoryCompat.IsModPresent)
			{
				Plugin.logger.LogDebug("Forcefully removing InventoryBuy node");
				Nodes.RemoveAll(node => node.name.ToLower() == "InventoryBuy".ToLower());
			}

			ContentManager.Nodes = Nodes;

			ContentManager.Keywords = terminal.terminalNodes.allKeywords.ToList();

			#endregion

			#region Routes

			List<SelectableLevel> levels = MrovLib.SharedMethods.GetGameLevels();

			for (int i = 0; i < levels.Count; i++)
			{
				SelectableLevel level = levels[i];

				List<TerminalNode> possibleNodes = Nodes.Where(x => x.buyRerouteToMoon == i || x.displayPlanetInfo == i).Distinct().ToList();

				if (MrovLib.Plugin.LLL.IsModPresent && possibleNodes.Count > 2)
				{
					List<TerminalNode> LLLNodes = MrovLib.SharedMethods.GetLevelTerminalNodes(level);

					possibleNodes.RemoveAll(node => !LLLNodes.Contains(node));
				}

				for (int j = 0; j < possibleNodes.Count; j++)
				{
					Plugin.DebugLogger.LogDebug($"Node: {possibleNodes[j]}");

					if (possibleNodes[j] == null)
					{
						continue;
					}
				}

				RelatedNodes relatedNodes =
					new()
					{
						Node = possibleNodes.Where(node => node.buyRerouteToMoon == -2).Distinct().ToList().FirstOrDefault(),
						NodeConfirm = possibleNodes.Where(node => node.buyRerouteToMoon != -2).Distinct().ToList().LastOrDefault()
					};

				ContentManager.Routes.Add(new Route(level, relatedNodes));
			}

			#endregion

			#region Items

			List<Item> buyableItems = terminal.buyableItemsList.ToList();

			for (int it = 0; it < buyableItems.Count; it++)
			{
				Item item = buyableItems[it];

				Plugin.DebugLogger.LogDebug($"Item: {item.itemName}");

				Plugin.DebugLogger.LogDebug($"Item index: {buyableItems.IndexOf(item)}");
				Plugin.DebugLogger.LogDebug($"Is terminal null: {terminal == null}");

				List<TerminalNode> possibleNodes = Nodes.Where(x => x.buyItemIndex == buyableItems.IndexOf(item)).ToList();

				Plugin.DebugLogger.LogDebug($"Possible nodes count: {possibleNodes.Count}");

				for (int i = 0; i < possibleNodes.Count; i++)
				{
					Plugin.DebugLogger.LogDebug($"Node: {possibleNodes[i]}");

					if (possibleNodes[i] == null)
					{
						continue;
					}
				}

				RelatedNodes relatedNodes =
					new()
					{
						Node = possibleNodes.Distinct().ToList().First(node => node.isConfirmationNode),
						NodeConfirm = possibleNodes.Distinct().ToList().First(node => !node.isConfirmationNode)
					};

				ContentManager.Buyables.Add(new BuyableItem(terminal, relatedNodes));
			}

			#endregion
			#region Unlockables, Decorations, Suits

			List<UnlockableItem> unlockables = StartOfRound.Instance.unlockablesList.unlockables.ToList();

			for (int i = 0; i < unlockables.Count; i++)
			{
				UnlockableItem unlockable = unlockables[i];

				Plugin.DebugLogger.LogDebug($"Unlockable: {unlockable.unlockableName}");
				List<TerminalNode> possibleNodes = Nodes.Where(x => x.shipUnlockableID == unlockables.IndexOf(unlockable)).Distinct().ToList();

				// super-quick check if the unlockable is a suit or decoration
				if (unlockable.suitMaterial != null)
				{
					// suit
					Plugin.DebugLogger.LogDebug($"Suit material: {unlockable.suitMaterial.name}");

					RelatedNodes suitRelatedNodes =
						new()
						{
							Node = possibleNodes.Where(node => !node.buyUnlockable).Distinct().ToList().FirstOrDefault(),
							NodeConfirm = possibleNodes.Where(node => node.buyUnlockable).Distinct().ToList().LastOrDefault()
						};

					if (ContentManager.Suits.Any(buyable => buyable.SuitMaterial == unlockable.suitMaterial))
					{
						Plugin.DebugLogger.LogWarning($"Suit {unlockable.unlockableName} already exists in the buyables list - skipping!");
						continue;
					}

					ContentManager.Buyables.Add(new BuyableSuit(terminal, suitRelatedNodes, unlockable));

					continue;
				}

				if (CheckPossibleNodeNull(possibleNodes))
				{
					continue;
				}

				RelatedNodes relatedNodes =
					new()
					{
						Node = possibleNodes.Where(node => !node.buyUnlockable).Distinct().ToList().FirstOrDefault(),
						NodeConfirm = possibleNodes.Where(node => node.buyUnlockable).Distinct().ToList().LastOrDefault()
					};

				if (relatedNodes.Node == null || relatedNodes.NodeConfirm == null)
				{
					continue;
				}

				if (unlockable.shopSelectionNode != null && !unlockable.alwaysInStock)
				{
					// decoration
					ContentManager.Buyables.Add(new BuyableDecoration(terminal, relatedNodes));
				}
				else
				{
					ContentManager.Buyables.Add(new BuyableUnlockable(terminal, relatedNodes));
				}
			}

			#endregion
			#region Vehicles

			List<BuyableVehicle> buyableVehicles = terminal.buyableVehicles.ToList();

			for (int i = 0; i < buyableVehicles.Count; i++)
			{
				BuyableVehicle vehicle = buyableVehicles[i];

				Plugin.DebugLogger.LogDebug($"Vehicle: {vehicle.vehicleDisplayName}");

				List<TerminalNode> possibleNodes = Nodes.Where(x => x.buyVehicleIndex == buyableVehicles.IndexOf(vehicle)).Distinct().ToList();

				if (CheckPossibleNodeNull(possibleNodes))
				{
					// Plugin.DebugLogger.LogDebug("Possible nodes are null");
					continue;
				}

				RelatedNodes relatedNodes =
					new()
					{
						Node = possibleNodes.Where(node => node.isConfirmationNode).Distinct().ToList().FirstOrDefault(),
						NodeConfirm = possibleNodes.Where(node => !node.isConfirmationNode).Distinct().ToList().LastOrDefault()
					};

				if (relatedNodes.Node == null || relatedNodes.NodeConfirm == null)
				{
					continue;
				}

				ContentManager.Buyables.Add(new BuyableCar(terminal, relatedNodes));
			}

			#endregion

			ContentManager.RouteDictionary.PopulateDictionary(ContentManager.Routes);

			List<Item> allItemsList = StartOfRound.Instance.allItemsList.itemsList.Distinct().ToList();
			foreach (Item item in allItemsList)
			{
				new Scrap(item);
			}

			List<TerminalNode> creatureNodes = Terminal.enemyFiles;
			foreach (TerminalNode node in creatureNodes)
			{
				Regex regex = new(@"s$");
				Creature creature = new() { Name = regex.Replace(node.creatureName, ""), InfoNode = node };

				Creatures.Add(creature);
			}

			// TODO

			// vanilla bundle: survival kit

			// new BuyableBundle(terminal, null, "Survival Kit", ["flashlight", "shovel"]);
		}

		internal static bool CheckPossibleNodeNull(List<TerminalNode> possibleNodes)
		{
			List<TerminalNode> Nodes = [];

			for (int j = 0; j < possibleNodes.Count; j++)
			{
				Plugin.DebugLogger.LogDebug($"Node: {possibleNodes[j]}");

				// somehow call continue on the upper loop

				if (possibleNodes[j] == null)
				{
					continue;
				}

				if (possibleNodes[j].itemCost <= 0)
				{
					continue;
				}

				Nodes.Add(possibleNodes[j]);
			}

			return possibleNodes == Nodes;
		}
	}
}
