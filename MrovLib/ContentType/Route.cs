using UnityEngine;

namespace MrovLib.ContentType
{
	public class Route : IBuyable
	{
		public string Name { get; set; }
		public int Price => this.Nodes.Node != null ? this.Nodes.Node.itemCost : 0;

		public SelectableLevel Level;
		public RelatedNodes Nodes;

		public Route(SelectableLevel level, RelatedNodes nodes)
		{
			Level = level;
			Nodes = nodes;

			Name = MrovLib.SharedMethods.GetNumberlessPlanetName(level);

			Plugin.DebugLogger.LogWarning($"Route constructor: {level}; {nodes.Node}, {nodes.NodeConfirm}");
		}

		public override string ToString()
		{
			return $"{Name}";
		}
	}
}
