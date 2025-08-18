using UnityEngine.SceneManagement;

namespace MrovLib.Patches
{
	internal static class SceneManagerPatches
	{
		private static bool WasLoadedBefore = false;

		internal static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			var currentScene = scene.name;

			if (currentScene is "InitSceneLaunchOptions")
			{
				EventManager.LaunchOptionsLoaded.Invoke();
			}

			if (currentScene is "MainMenu")
			{
				if (WasLoadedBefore)
				{
					return;
				}

				EventManager.MainMenuLoaded.Invoke();
				WasLoadedBefore = true;
			}

			EventManager.SceneLoaded.Invoke(currentScene);
		}
	}
}
