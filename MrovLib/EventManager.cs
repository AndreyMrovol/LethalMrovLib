namespace MrovLib
{
	public static class EventManager
	{
		public static MrovLib.Events.CustomEvent<Terminal> TerminalStart = new();
		public static MrovLib.Events.CustomEvent<StartOfRound> LobbyDisabled = new();

		public static MrovLib.Events.CustomEvent LaunchOptionsLoaded = new();
		public static MrovLib.Events.CustomEvent MainMenuLoaded = new();
		public static MrovLib.Events.CustomEvent<string> SceneLoaded = new();

		public static MrovLib.Events.CustomEvent ContentManagerReady = new();
	}
}
