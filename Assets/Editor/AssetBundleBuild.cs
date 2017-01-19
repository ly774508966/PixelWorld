using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class AssetBundleBuild : Editor 
{
	const string kAssetBundlesOutputPath = "AssetBundles";

	[MenuItem("AssetBundle/Build All")]
	private static void BuildAll()
	{
		string path = Path.Combine(kAssetBundlesOutputPath, AssetBundleManager.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget));

		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}

		BuildPipeline.BuildAssetBundles(path, 0, EditorUserBuildSettings.activeBuildTarget);
	}

}