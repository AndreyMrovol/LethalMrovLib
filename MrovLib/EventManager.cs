namespace MrovLib
{
	public static class EventManager
	{
		public static MrovLib.Events.CustomEvent<Terminal> TerminalStart = new();
		public static MrovLib.Events.CustomEvent<StartOfRound> LobbyDisabled = new();
	}
}
