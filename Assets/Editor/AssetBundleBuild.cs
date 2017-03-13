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
	const string kAssetBundlesOutputPath = "Update/AssetBundles";

	[MenuItem("AssetBundle/Generate Bundle Names")]
	private static void GenerateNames()
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


		// resources
		string resourceRoot = "Assets/Resources/";
		string[] dirs = Directory.GetDirectories(resourceRoot);
		foreach (string dir in dirs) {
			string relative = dir.Remove(0, resourceRoot.Length).ToLower();
			if (relative == "lua") continue;	// lua单独处理

			if (relative == "sprite") { // 合并
				GenNameRecursively(resourceRoot, relative, false);
			} else {
				GenNameRecursively(resourceRoot, relative, true);
			}
		}

		// lua
		string luaRoot = "Assets/Resources/Lua";
		List<string> dirs2 = new List<string>();
		GetAllDirs(luaRoot, dirs2);
 
		for (int i = 0; i < dirs2.Count; i++) {
			string str = dirs2[i].Remove(0, luaRoot.Length);
			BuildLuaBundle(str.Replace('\\', '/'), luaRoot);
		}
		BuildLuaBundle(null, luaRoot);

		// ugui atlas
		GenNameRecursively("Assets/", "Arts/", false);

		Debug.Log("Generate names ok");

		// save
		AssetDatabase.SaveAssets();
	}
	static void GenNameRecursively(string root, string dir, bool bSeparately) {
		string path = Path.Combine(root, dir);

		// files
		string[] files = Directory.GetFiles(path);
		foreach( string file in files) {
			string bundleName = dir;
			if (bSeparately) {
				bundleName += "/"+Path.GetFileNameWithoutExtension(file);
			}

			AssetImporter importer = AssetImporter.GetAtPath(file);
			if (importer) {
				importer.assetBundleName = bundleName;
				importer.assetBundleVariant = null;                
			}
		}

		// dirs recusively
		string[] dirs = Directory.GetDirectories(path);
		for (int i = 0; i < dirs.Length; i++) {
			string str = dirs[i].Remove(0, root.Length);
			GenNameRecursively(root, str.Replace('\\', '/'), bSeparately);
		}
	}
	static void GetAllDirs(string dir, List<string> list)
	{
		string[] dirs = Directory.GetDirectories(dir);
		list.AddRange(dirs);

		for (int i = 0; i < dirs.Length; i++) {
			GetAllDirs(dirs[i], list);
		}
	}
	static void BuildLuaBundle(string subDir, string sourceDir)
	{
		string[] files = Directory.GetFiles(sourceDir + subDir, "*.bytes");
		string bundleName = subDir == null ? "lua.unity3d" : "lua" + subDir.Replace('/', '_') + ".unity3d";
		bundleName = bundleName.ToLower();
      
		for (int i = 0; i < files.Length; i++) {
			AssetImporter importer = AssetImporter.GetAtPath(files[i]);
			if (importer) {
				importer.assetBundleName = bundleName;
				importer.assetBundleVariant = null;                
			}
		}       
	}

	[MenuItem("AssetBundle/Clear Bundle Names")]
	private static void ClearNames()
	{
		Stack<string> dirs = new Stack<string>();
		dirs.Push("Assets/Resources/");
		while(dirs.Count > 0) {
			string dir = dirs.Pop();
			// files
			foreach( string file in Directory.GetFiles(dir)) {
				
				AssetImporter importer = AssetImporter.GetAtPath(file);
				if (importer)
				{
					importer.assetBundleName = null;              
				}
			}
			// child dirs
			foreach( string path in Directory.GetDirectories(dir)){
				dirs.Push(path);
			}
		}

		Debug.Log("clear names ok");
		AssetDatabase.SaveAssets();
	}

	[MenuItem("AssetBundle/Build All (Base)")]
	private static void BuildBase()
	{
		string PlatformFolder = AssetBundleManager.GetPlatformFolderForAssetBundles (EditorUserBuildSettings.activeBuildTarget);
		string path = Path.Combine(kAssetBundlesOutputPath, PlatformFolder);

		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}

		AssetBundleManifest manifest =  BuildPipeline.BuildAssetBundles(path, 0, EditorUserBuildSettings.activeBuildTarget);
		if (manifest == null) {
			EditorUtility.DisplayDialog ("警告", "没有需要打包的assetbundle，请先设置assetbundle的名字！", "确定");
			return;
		}

		// generate resourcelist
		string filename = Path.Combine(path, "base.txt");
		FileStream fs = new FileStream(filename, FileMode.Create);
		StreamWriter writer = new StreamWriter(fs);
		writer.WriteLine(string.Format("version 1.0.0"));
		writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

		// write assetbundles
		string[] assets = manifest.GetAllAssetBundles();
		foreach (string asset in assets) {
			// assetbundle 
			Hash128 hash = manifest.GetAssetBundleHash(asset);
			long size = ResourceManager.GetFileSize(Path.Combine(path, asset));
			writer.WriteLine(string.Format("{0} {1} {2}", asset, hash.ToString(), size));
		}

		writer.Close();
		fs.Close();

		/*
		// delete generated files
		foreach( string file in Directory.GetFiles(path)) {
			if (file != filename) {
				File.Delete(file);
			}
		}
		// child dirs
		foreach( string dir in Directory.GetDirectories(path)){
			FileUtil.DeleteFileOrDirectory(dir);
		}
		*/


		Debug.Log("build base ok");
	}

	[MenuItem("AssetBundle/Build All (Inc)")]
	private static void BuildInc()
	{
		string PlatformFolder = AssetBundleManager.GetPlatformFolderForAssetBundles (EditorUserBuildSettings.activeBuildTarget);
		string path = Path.Combine(kAssetBundlesOutputPath, PlatformFolder);

		string filename = Path.Combine(path, "base.txt");
		if (!File.Exists(filename)) {
			Debug.LogError("no base file exist!");
			return;
		}

		// read base
		Dictionary<string, string> files_base = new Dictionary<string, string>();
		FileStream fs = new FileStream(filename, FileMode.Open);
		StreamReader reader = new StreamReader(fs);
		string version = reader.ReadLine();
		string date = reader.ReadLine();
		string line;
		while((line = reader.ReadLine()) != null) {
			string[] strs = line.Split(' ');
			if (strs.Length != 3) {
				Debug.Log("error format!");
			} else {
				files_base.Add(strs[0], strs[1]);
			}
		}
		reader.Close();
		fs.Close();

		// new assetbundle
		AssetBundleManifest manifest =  BuildPipeline.BuildAssetBundles(path, 0, EditorUserBuildSettings.activeBuildTarget);
		if (manifest == null) {
			EditorUtility.DisplayDialog ("警告", "没有需要打包的assetbundle，请先设置assetbundle的名字！", "确定");
			return;
		}

		// compare assetbundles
		string[] assets = manifest.GetAllAssetBundles();
		HashSet<string> hashSet = new HashSet<string>();
		foreach (string asset in assets) {
			// already added
			if (hashSet.Contains(asset))  continue;

			// check assetbundle 
			Hash128 hash = manifest.GetAssetBundleHash(asset);
			long size = ResourceManager.GetFileSize(Path.Combine(path, asset));
			if (files_base.ContainsKey(asset)) {
				if (files_base[asset].CompareTo(hash.ToString()) == 0) { // no change
					continue;
				}
			}

			// add changed
			hashSet.Add(asset);

			// add dependencies
			string[] depends = manifest.GetAllDependencies(asset);
			foreach(string depend in depends) {
				if (hashSet.Contains(depend)) continue;
				hashSet.Add(depend);
			}
		}

		// exclude no change assetbundle
		Stack<string> dirs = new Stack<string>();
		dirs.Push("Assets/Resources/");
		while(dirs.Count > 0) {
			string dir = dirs.Pop();
			// files
			foreach( string file in Directory.GetFiles(dir)) {
				
				AssetImporter importer = AssetImporter.GetAtPath(file);
				if (importer) {
					if (!hashSet.Contains( importer.assetBundleName)){
						importer.assetBundleName = null;
					}          
				}
			}
			// child dirs
			foreach( string p in Directory.GetDirectories(dir)){
				dirs.Push(p);
			}
		}
		// delete generated files
		foreach( string file in Directory.GetFiles(path)) {
			if (file == Path.Combine(path, "base.txt") || file == Path.Combine(path, "resourcelist.txt")) {
				continue;
			}
			File.Delete(file);
		}
		// child dirs
		foreach( string dir in Directory.GetDirectories(path)){
			FileUtil.DeleteFileOrDirectory(dir);
		}

		// refresh
		AssetDatabase.Refresh();

		manifest =  BuildPipeline.BuildAssetBundles(path, 0, EditorUserBuildSettings.activeBuildTarget);

		// generate resourcelist
		filename = Path.Combine(path, "resourcelist.txt");
		fs = new FileStream(filename, FileMode.Create);
		StreamWriter writer = new StreamWriter(fs);
		writer.WriteLine(string.Format("version 1.0.0"));
		writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

		if (manifest == null) {
			writer.Close();
			fs.Close();
			Debug.Log("no changed assetbundle");
			return;
		}

		assets = manifest.GetAllAssetBundles();
		Debug.Assert(assets.Length == hashSet.Count);

		if (assets.Length > 0) {
			// compare manifest
			writer.WriteLine(string.Format("{0} {1} {2}",
				PlatformFolder,
				ResourceManager.GetFileMD5(Path.Combine(path, PlatformFolder)), 
				ResourceManager.GetFileSize(Path.Combine(path, PlatformFolder))));

			foreach (string asset in assets) {
				Hash128 hash = manifest.GetAssetBundleHash(asset);
				long size = ResourceManager.GetFileSize(Path.Combine(path, asset));

				writer.WriteLine(string.Format("{0} {1} {2}", asset, hash.ToString(), size));
			}
		}

		writer.Close();
		fs.Close();

		Debug.Log("build inc ok");
	}


	private static void DeleteEmptyFolder(string path) {
		foreach( string dir in Directory.GetDirectories(path)){
			DeleteEmptyFolder(dir);
		}
	
		string[] paths =  Directory.GetDirectories(path);
		string[] files =  Directory.GetFiles(path);
		if (files.Length == 0 && paths.Length == 0) {
			Directory.Delete(path);
		}
	}
}