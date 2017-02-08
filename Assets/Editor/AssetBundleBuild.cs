using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;  
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class AssetBundleBuild : Editor 
{
	const string kAssetBundlesOutputPath = "AssetBundles";

	[MenuItem("AssetBundle/Generate Name")]
	private static void GenerateName()
	{
		string filename = Path.Combine("Update", "AssetBundleRule.txt");
		if (!File.Exists(filename)) {
			Debug.LogError("no rule exist!");
		}
		// read rules
		Dictionary<string, int> Rules = new Dictionary<string, int>();
		FileStream fs = new FileStream(filename, FileMode.Open);
		StreamReader reader = new StreamReader(fs);
		string line;
		while((line = reader.ReadLine()) != null) {
			string[] strs = line.Split(' ');
		}
		reader.Close();
		fs.Close();

		// check files
		Stack<string> dirs = new Stack<string>();
		dirs.Push("Assets/Resources/");
		while(dirs.Count > 0) {
			string dir = dirs.Pop();
			// files
			foreach( string file in Directory.GetFiles(dir)) {
				if (Path.GetExtension(file) != ".meta") {
					string name = file.Split('.')[0].Substring(17);

					if (name=="resourcelist") continue;

					Debug.Log(name);
					AssetImporter.GetAtPath(file).assetBundleName = name;
				}
			}
			// child dirs
			foreach( string path in Directory.GetDirectories(dir)){
				dirs.Push(path);
			}
		}
	}

	[MenuItem("AssetBundle/Build Base")]
	private static void BuildBase()
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
		writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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

	[MenuItem("AssetBundle/Build Inc")]
	private static void BuildInc()
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
		writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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