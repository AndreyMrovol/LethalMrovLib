namespace MrovLib.ContentType
{
	public class Creature
	{
		public string Name { get; set; }
		public TerminalNode InfoNode { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
