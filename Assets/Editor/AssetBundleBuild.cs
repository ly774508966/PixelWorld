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

					if (name=="version") continue;

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
		string filename = Path.Combine(path, "base.txt");
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
	}

	[MenuItem("AssetBundle/Build Inc")]
	private static void BuildInc()
	{

		string path = Path.Combine(kAssetBundlesOutputPath, AssetBundleManager.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget));

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

		string[] files = manifest.GetAllAssetBundles();

		// generate resourcelist
		filename = Path.Combine(path, "resourcelist.txt");
		fs = new FileStream(filename, FileMode.Create);
		StreamWriter writer = new StreamWriter(fs);
		writer.WriteLine(string.Format("version 1.0.0"));
		writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

		List<string> files_diff = new List<string>();
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
			hashSet.Add(asset);

			// no depends , write
			Hash128 hash = manifest.GetAssetBundleHash(asset);
			long size = ResourceManager.GetInstance().GetFileSize(Path.Combine(path, asset));

			if (files_base.ContainsKey(asset)) {
				if (files_base[asset].CompareTo(hash.ToString()) == 0) {
					continue;
				}
			}
			writer.WriteLine(string.Format("{0} {1} {2}", asset, hash.ToString(), size));
			files_diff.Add(Path.Combine(path, asset.Replace("/", "\\")));
		}

		writer.Close();
		fs.Close();

		// remove useless files
		Stack<string> dirs = new Stack<string>();
		dirs.Push(path);
		while(dirs.Count > 0) {
			string dir = dirs.Pop();
			// files
			foreach( string file in Directory.GetFiles(dir)) {
				if (file == Path.Combine(path, "base.txt") || file == Path.Combine(path, "resourcelist.txt")) {
					continue;
				}
				if (Path.GetExtension(file) == ".manifest") {
					FileUtil.DeleteFileOrDirectory(file);
					continue;
				}

				if (files_diff.Contains(file) == false) {
					FileUtil.DeleteFileOrDirectory(file);
					continue;
				}
			}
			// child dirs

			foreach( string p in Directory.GetDirectories(dir)){
				dirs.Push(p);
			}
		}

		DeleteEmptyFolder(path);
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