namespace MrovLib.ContentType
{
	public class Creature
	{
		public string Name { get; set; }
		public TerminalNode InfoNode { get; set; }
		public bool Discovered => ContentManager.Terminal.scannedEnemyIDs.Contains(InfoNode.creatureFileID);

		public override string ToString()
		{
			return Name;
		}
	}
}
