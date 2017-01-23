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

		AssetBundleManifest manifest =  BuildPipeline.BuildAssetBundles(path, 0, EditorUserBuildSettings.activeBuildTarget);

		string[] files = manifest.GetAllAssetBundles();

		// generate resourcelist
		string filename = Path.Combine(path, "resourcelist.txt");
		FileStream fs = new FileStream(filename, FileMode.Create);
		StreamWriter writer = new StreamWriter(fs);
		writer.WriteLine(string.Format("version 1.0.0"));

		HashSet<string> hashSet = new HashSet<string>();
		Stack<string> stack = new Stack<string>();
		for(int i = 0; i < files.Length; i ++) stack.Push(files[i]);
		while(stack.Count > 0) {
			string asset = stack.Peek();
			if (hashSet.Contains(asset)) {
				stack.Pop();
				continue;
			}

			// check depends first
			string[] depends = manifest.GetAllDependencies(asset);
			if (depends.Length > 0) {
				bool bHas = false;
				for (int i = 0 ; i < depends.Length; i ++) {
					if (!hashSet.Contains(depends[i])) {
						stack.Push(depends[i]);
						bHas = true;
					}
				}
				if (bHas) continue;
			} 

			// no depends or depends add already
			stack.Pop();
			// no depends , write
			Hash128 hash = manifest.GetAssetBundleHash(asset);
			long size = ResourceManager.GetInstance().GetFileSize(Path.Combine(path, asset));
			writer.WriteLine(string.Format("{0} {1} {2}", asset, hash.ToString(), size));
			hashSet.Add(asset);
			
		}

		writer.Close();
		fs.Close();
	}
}