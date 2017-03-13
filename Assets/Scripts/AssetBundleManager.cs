using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;

public delegate void HandleDownloadFinish(WWW www);
public delegate void HandleDownloadCallback();

public class AssetBundleManager : MonoBehaviour {

	private class LoadedAssetBundle {
		public AssetBundle assetBundle;
		public int refCount;
		public LoadedAssetBundle(AssetBundle assetBundle) {
			this.assetBundle = assetBundle;
			refCount = 1;
		}
		public void Unload() {
			assetBundle.Unload (false);
		}
	}

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
			s_BaseDownloadingURL +=
#if UNITY_EDITOR
			GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			GetPlatformFolderForAssetBundles(Application.platform);
#endif
			s_BaseDownloadingURL += "/";
			Debug.Log("AssetBundleManager baseURL " + s_BaseDownloadingURL);
		}
		return instance;
	}


	static string s_BaseDownloadingURL = GameConfig.WebUrl;
	public string BaseDownloaindURL {get {return s_BaseDownloadingURL;}}


	static Queue<string> s_ToDownloadAssetBundles = new Queue<string>();
	static Dictionary<string, WWW> s_DownloadingWWWs = new Dictionary<string, WWW>();

	// assetbundles
	static Dictionary<string, string[]> s_AssetBundleDependencies = new Dictionary<string, string[]>();
	static Dictionary<string, LoadedAssetBundle> s_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();


	static HandleDownloadCallback m_callback;

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

	public static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
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

	public static string GetPlatformFolderForAssetBundles() {
#if UNITY_EDITOR
		return GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
		return GetPlatformFolderForAssetBundles(Application.platform);
#endif
	}

	public static int GetToDownloadAssetBundleNum() {
		return s_ToDownloadAssetBundles.Count;
	}
	public static void AddDownloadAssetBundle(string assetBundleName) {
		s_ToDownloadAssetBundles.Enqueue(assetBundleName);
	}

	public static int GetDownloadingWWWNum() {
		return s_DownloadingWWWs.Count;
	}

	public static void InitDependenceInfo() {
		Debug.Log ("InitDependenceInfo");

		string filename = Path.Combine(Application.persistentDataPath, AssetBundleManager.GetPlatformFolderForAssetBundles());
		if (File.Exists (filename)) {
			AssetBundle assetBundle = AssetBundle.LoadFromFile (filename);
			AssetBundleManifest manifest = assetBundle.LoadAsset ("assetbundlemanifest") as AssetBundleManifest;
			string[] assetBundleNames = manifest.GetAllAssetBundles ();
			foreach (string assetBundleName in assetBundleNames) {
				string[] dependencies = manifest.GetAllDependencies(assetBundleName);
				s_AssetBundleDependencies.Add (assetBundleName, dependencies);
			}
			assetBundle.Unload (true);

		} else {
			Debug.LogWarning ("cannot find Manifest file");
		}
	}

	// load assetbuddle 
	// ref++
	public static AssetBundle GetAssetBundle(string assetBundleName) {
		if (s_AssetBundleDependencies.ContainsKey(assetBundleName)) {
			string[] dependencies = s_AssetBundleDependencies [assetBundleName];
			foreach (string dependency in dependencies) {
				if (s_LoadedAssetBundles.ContainsKey (dependency)) {
					s_LoadedAssetBundles [dependency].refCount++;
				} else {
					// 加载
					string filename = Path.Combine(Application.persistentDataPath, dependency);
					if (File.Exists(filename)) {
						AssetBundle ab = AssetBundle.LoadFromFile(filename);
						if (ab) {
							Debug.LogFormat("AssetBundle(Dependency) loaded : {0}", dependency);
							LoadedAssetBundle loadedAssetBundle = new LoadedAssetBundle (ab);
							s_LoadedAssetBundles.Add (dependency, loadedAssetBundle);
							continue;
						}
					}
					// ab not found
					Debug.LogErrorFormat("AssetBundle(Dependency) not found : {0}", dependency);
				}
			}
		}

		if (s_LoadedAssetBundles.ContainsKey (assetBundleName)) {

			s_LoadedAssetBundles [assetBundleName].refCount++;

			return s_LoadedAssetBundles [assetBundleName].assetBundle;
		} else {
			// 加载
			string filename = Path.Combine(Application.persistentDataPath, assetBundleName);
			if (File.Exists(filename)) {
				AssetBundle ab = AssetBundle.LoadFromFile(filename);
				if (ab) {
					Debug.LogFormat("AssetBundle loaded : {0}", assetBundleName);
					LoadedAssetBundle loadedAssetBundle = new LoadedAssetBundle (ab);
					s_LoadedAssetBundles.Add (assetBundleName, loadedAssetBundle);
					return s_LoadedAssetBundles [assetBundleName].assetBundle;
				}
			}
		
			return null;
		}
	}
	public static void UnloadAssetBundle(string assetBundleName) {
		if (s_AssetBundleDependencies.ContainsKey(assetBundleName)) {
			string[] dependencies = s_AssetBundleDependencies [assetBundleName];
			foreach (string dependency in dependencies) {
				if (s_LoadedAssetBundles.ContainsKey (dependency)) {
					s_LoadedAssetBundles [dependency].refCount--;
					if (s_LoadedAssetBundles [dependency].refCount == 0) {
						s_LoadedAssetBundles [dependency].Unload ();
						s_LoadedAssetBundles.Remove (dependency);
					}
				}
			}
		}

		if (s_LoadedAssetBundles.ContainsKey (assetBundleName)) {

			s_LoadedAssetBundles [assetBundleName].refCount--;

			if (s_LoadedAssetBundles [assetBundleName].refCount == 0) {
				s_LoadedAssetBundles [assetBundleName].Unload ();
				s_LoadedAssetBundles.Remove (assetBundleName);
			}
		}
	}
		
	public void SetDownloadCallback(HandleDownloadCallback callback) {
		m_callback = callback;
	}

	public void LoadAssetBundleLocal(string assetBundleName) {
		Debug.Log("LoadAssetBundleLocal " + assetBundleName);

		StartCoroutine( DownloadAssetBundle(Path.Combine("file:///"+Application.persistentDataPath, assetBundleName), assetBundleName, delegate (WWW www){

			//m_LoadedAssetBundles.Add(assetBundleName, www.assetBundle);

		}
		));
	}

	private void LoadAssetBundle(string assetBundleName) {
		Debug.Log("LoadAssetBundle " + assetBundleName);
		StartCoroutine( DownloadAssetBundle(s_BaseDownloadingURL+assetBundleName, assetBundleName, delegate (WWW www){

			// write to local 
			WriteToLocal(assetBundleName, www.bytes);	

		}
		));
	}

 	IEnumerator DownloadAssetBundle(string url, string assetBundleName, HandleDownloadFinish handler) {

		Debug.Log("start downloading " + url);

		WWW www = new WWW(url);
		s_DownloadingWWWs.Add(assetBundleName, www);

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
		s_DownloadingWWWs.Remove(assetBundleName);
		www.Dispose();
	}

	void Update() {
		if (s_DownloadingWWWs.Count < 5) {
			if (s_ToDownloadAssetBundles.Count > 0) {
				string assetBundleName = s_ToDownloadAssetBundles.Dequeue();
				LoadAssetBundle(assetBundleName);

				if (m_callback != null) m_callback();
			}
		}
	}

	private void WriteToLocal(string name, byte [] data) {
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
	}
}