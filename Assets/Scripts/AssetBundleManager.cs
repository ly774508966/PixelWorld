using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class AssetBundleManager : MonoBehaviour {

	private static AssetBundleManager instance;
	public static AssetBundleManager GetInstance() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
	
		if (instance == null) {
			instance = main.AddComponent<AssetBundleManager>();

			// download path for platforms
			m_BaseDownloadingURL +=
#if UNITY_EDITOR
			GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			GetPlatformFolderForAssetBundles(Application.platform);
#endif
			m_BaseDownloadingURL += "/";
			Debug.Log("AssetBundleManager baseURL " + m_BaseDownloadingURL);
		}
		return instance;
	}

	public delegate void HandleDownloadFinish(WWW www);

	static string m_BaseDownloadingURL = "http://192.168.1.106:8080/update/AssetBundles/";


	static Queue<string> m_ToLoadAssetBundles = new Queue<string>();
	static Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW>();
	static Dictionary<string, AssetBundle> m_LoadedAssetBundles = new Dictionary<string, AssetBundle>();

	public Dictionary<string, AssetBundle> LoadedAssetBundles {get {return m_LoadedAssetBundles;}}

#if UNITY_EDITOR
	public static string GetPlatformFolderForAssetBundles(BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "Android";
		case BuildTarget.iOS:
			return "iOS";
		case BuildTarget.WebPlayer:
			return "WebPlayer";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "Windows";
		case BuildTarget.StandaloneOSXIntel:
		case BuildTarget.StandaloneOSXIntel64:
		case BuildTarget.StandaloneOSXUniversal:
			return "OSX";
		default:
			return null;
		}
	}
#endif

	static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
	{
		switch(platform) {
		case RuntimePlatform.Android:
			return "Android";
		case RuntimePlatform.IPhonePlayer:
			return "iOS";
		case RuntimePlatform.WindowsWebPlayer:
		case RuntimePlatform.OSXWebPlayer:
			return "WebPlayer";
		case RuntimePlatform.WindowsPlayer:
			return "Windows";
		case RuntimePlatform.OSXPlayer:
			return "OSX";
		default:
			return null;
		}
	}

	public int GetToLoadAssetBundleNum() {
		return m_ToLoadAssetBundles.Count;
	}

	public int GetDownloadingWWWNum() {
		return m_DownloadingWWWs.Count;
	}


	public AssetBundle GetLoadedAssetBundle(string assetBundleName) {
		if (m_LoadedAssetBundles.ContainsKey(assetBundleName) == false) {
			Debug.Log("not loaded for " + assetBundleName);
			return null;
		}

		return m_LoadedAssetBundles[assetBundleName];
	}

	public void AddLoadAssetBundle(string assetBundleName) {
		m_ToLoadAssetBundles.Enqueue(assetBundleName);
	}

	public void LoadAssetBundle(string assetBundleName) {
		Debug.Log("LoadAssetBundle " + assetBundleName);
		StartCoroutine( DownloadAssetBundle(assetBundleName, delegate (WWW www){

			m_LoadedAssetBundles.Add(assetBundleName, www.assetBundle);

			// write to local 
			WriteToLocal(assetBundleName, www.bytes);	
		}
		));
	}

 	IEnumerator DownloadAssetBundle(string assetBundleName, HandleDownloadFinish handler) {

		string url = m_BaseDownloadingURL + assetBundleName;
		Debug.Log("start downloading " + url);

		WWW www = new WWW(url);
		m_DownloadingWWWs.Add(assetBundleName, www);

		yield return www;

		if (www.error != null) {
			Debug.LogError("downloading error! " + www.error);
		} else {
			if (www.isDone) {
				if (handler != null) {
					handler(www);
				}
			}
		}

		// destroy
		m_DownloadingWWWs.Remove(assetBundleName);
		www.Dispose();
	}

	void Update() {
		if (m_DownloadingWWWs.Count < 5) {
			if (m_ToLoadAssetBundles.Count > 0) {
				string assetBundleName = m_ToLoadAssetBundles.Dequeue();
				LoadAssetBundle(assetBundleName);
			}
		}
	}

	void WriteToLocal(string name, byte [] data) {
		Debug.Log("WriteToLocal " + name);
		string filename = Path.Combine(Application.persistentDataPath, name);
		if (!File.Exists(filename)) {
			string path = Path.GetDirectoryName(filename);
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}

		FileStream file = new FileStream(filename, FileMode.Create);
		file.Write(data, 0, data.Length);
		file.Close();

		//File.WriteAllBytes(filename, data);
	}
}