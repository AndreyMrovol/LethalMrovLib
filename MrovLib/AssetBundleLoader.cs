using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;

namespace MrovLib
{
	public abstract class AssetBundleLoaderManager
	{
		internal DirectoryInfo pluginsFolder = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent;
		internal Logger Logger = new("AssetBundleLoader", LoggingType.Debug);

		public virtual List<string> BundleExtensions { get; internal set; } = ["mrovbundle"];
		public virtual List<string> BundleBlacklist { get; internal set; } = [];

		public Dictionary<string, AssetBundle> LoadedBundles { get; internal set; } = [];

		public Dictionary<Type, AssetBundleLoader> AssetBundleLoadersByType { get; internal set; } = [];

		// TODO: display only loaded assetbundles that had loaded data inside
		public Dictionary<AssetBundle, int> LoadedAssetsCount { get; internal set; } = [];

		public List<T> GetLoadedAssets<T>()
			where T : UnityEngine.Object
		{
			if (AssetBundleLoadersByType.TryGetValue(typeof(T), out AssetBundleLoader loader))
			{
				if (loader is AssetBundleLoader<T> typedLoader)
				{
					return typedLoader.LoadedAssets;
				}
			}
			return [];
		}

		public void LoadAllBundles()
		{
			// Path where you'll place your asset bundles (next to the plugin DLL)
			string bundlesPath = pluginsFolder.FullName;

			Logger.LogDebug($"Loading asset bundles from: {bundlesPath}");

			if (!Directory.Exists(bundlesPath))
			{
				Logger.LogWarning($"AssetBundles folder not found: {bundlesPath}");
				return;
			}

			BundleExtensions.ForEach(ext => LoadAllBundlesInFolder(bundlesPath, ext));

			Logger.LogCustom(
				$"Loaded {LoadedBundles.Count} asset bundles: [{string.Join(", ", LoadedBundles.Keys)}]",
				LogLevel.Info,
				LoggingType.Basic
			);
		}

		private void LoadAllBundlesInFolder(string bundlesPath, string bundleExtension)
		{
			string[] bundleFiles = Directory.GetFiles(bundlesPath, $"*.{bundleExtension}", SearchOption.AllDirectories);

			Logger.LogInfo($"Found {bundleFiles.Length} asset bundles with extension {bundleExtension}");

			foreach (string bundlePath in bundleFiles)
			{
				string fileName = Path.GetFileName(bundlePath);
				LoadBundle(bundlePath, fileName);
			}
		}

		private void LoadBundle(string bundlePath, string bundleName)
		{
			try
			{
				AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

				if (bundle == null)
				{
					Logger.LogError($"Failed to load asset bundle: {bundleName}");
					return;
				}

				if (BundleBlacklist.Contains(bundle.name.Replace(".weatherbundle", "")))
				{
					Logger.LogWarning($"Asset bundle {bundle.name} is blacklisted, skipping loading!");
					bundle.UnloadAsync(true);
					return;
				}

				LoadedBundles[bundleName] = bundle;
				Logger.LogInfo($"Loaded asset bundle: {bundleName}");

				AssetBundleLoadersByType
					.Keys.ToList()
					.ForEach(LoaderType =>
					{
						AssetBundleLoader assetBundleLoader = AssetBundleLoadersByType[LoaderType];
						Logger.LogCustom(
							$"Loading assets of type {assetBundleLoader} from bundle {bundleName}",
							LogLevel.Debug,
							LoggingType.Developer
						);

						assetBundleLoader.LoadFromBundle(bundle);
					});
			}
			catch (System.Exception e)
			{
				Logger.LogError($"Error loading bundle {bundleName}: {e.Message}");
				Logger.LogCustom($"{e.StackTrace}", LogLevel.Error, LoggingType.Debug);
			}
		}

		public virtual void ConvertLoadedAssets()
		{
			throw new NotImplementedException();
		}
	}

	// this is SO BAD, but it's the only way to make a list with any generic types
	public abstract class AssetBundleLoader
	{
		public virtual void LoadFromBundle(AssetBundle bundle)
		{
			throw new NotImplementedException();
		}
	}

	public class AssetBundleLoader<T> : AssetBundleLoader
		where T : UnityEngine.Object
	{
		public Type AssetType => typeof(T);

		public List<T> LoadedAssets { get; private set; } = [];

		// Implement the abstract method from the base class
		public override void LoadFromBundle(AssetBundle bundle)
		{
			LoadedAssets.AddRange(LoadContentFromBundle(bundle));
		}

		public virtual List<T> LoadContentFromBundle(AssetBundle bundle)
		{
			List<T> LoadedAssets = bundle.LoadAllAssets<T>().ToList();
			return LoadedAssets;
		}

		public void Reset()
		{
			LoadedAssets = [];
		}

		public override string ToString()
		{
			return typeof(T).Name;
		}
	}
}
