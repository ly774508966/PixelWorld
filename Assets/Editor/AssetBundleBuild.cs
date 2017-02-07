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

	public static string getFileHash(string filePath) {             
		try   {  
			FileStream fs = new FileStream(filePath, FileMode.Open);  
			int len = (int)fs.Length;  
			byte[] data = new byte[len];  
			fs.Read(data, 0, len);  
			fs.Close();  
			MD5 md5 = new MD5CryptoServiceProvider();  
			byte[] result = md5.ComputeHash(data);  
			string fileMD5 = "";  
			foreach (byte b in result) {  
				fileMD5 += Convert.ToString(b, 16); 
			}  
			return fileMD5;     
		} catch (FileNotFoundException e) {  
			Debug.LogError(e.Message);  
			return "";  
		}                                   
        }  

	[MenuItem("AssetBundle/Generate MD5 File")]
	private static void GenerateMd5File()
	{
		// 生成md5文件
		string filename = Path.Combine("", "md5.txt");
		FileStream fs = new FileStream(filename, FileMode.Create);
		StreamWriter writer = new StreamWriter(fs);
		writer.WriteLine(string.Format("version 1.0.0"));


		Stack<string> dirs = new Stack<string>();
		dirs.Push("Assets");
		while(dirs.Count > 0) {
			string dir = dirs.Pop();

			// files
			foreach( string file in Directory.GetFiles(dir)) {
				if (Path.GetExtension(file) != ".meta") {
					Debug.Log(file);
					writer.WriteLine(string.Format("{0} {1}", file, getFileHash(file)));
				}
			}
			// child dirs
			foreach( string path in Directory.GetDirectories(dir)){
				dirs.Push(path);
			}
		}

		writer.Close();
		fs.Close();
	}

	[MenuItem("AssetBundle/Build Rules")]
	private static void BuildRules()
	{
		// 对比原始md5，得到修改的文件列表
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
	}

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