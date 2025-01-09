using UnityEngine;

namespace MrovLib.ContentType
{
	public class Route : ScriptableObject, IBuyable
	{
		public string Name { get; set; }
		public int Price => this.Nodes.Node != null ? this.Nodes.Node.itemCost : 0;

		public SelectableLevel Level;
		public RelatedNodes Nodes;

		public Route(SelectableLevel level, RelatedNodes nodes)
		{
			Level = level;
			Nodes = nodes;

			name = MrovLib.SharedMethods.GetNumberlessPlanetName(level);
			Name = name;

			Plugin.DebugLogger.LogWarning($"Route constructor: {level}; {nodes.Node}, {nodes.NodeConfirm}");
		}
	}
}
