using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ResourceManager : MonoBehaviour {

	private static ResourceManager instance;
	public static ResourceManager GetInstance() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
	
		if (instance == null) {
			instance = main.AddComponent<ResourceManager>();
		}
		return instance;
	}


	static Dictionary<string, AssetBundle> m_AssetMapAssetBundle = new Dictionary<string, AssetBundle>();

	public void Init() {
		Debug.Log("ResourceManager:Init");
		Dictionary<string, AssetBundle> assetBundles = AssetBundleManager.GetInstance().LoadedAssetBundles;

		foreach(AssetBundle assetBundle in assetBundles.Values) {
			string[] assets = assetBundle.GetAllAssetNames();
			for(int i = 0; i < assets.Length; i ++) {
				Debug.Log("asset " + assets[i]);

				int pos = assets[i].IndexOf("assets/resources/");
				if (pos != -1) {
					pos = assets[i].LastIndexOf(".");
					string assetName = assets[i].Substring(17, pos-17);
					Debug.Log("assetName " + assetName);
					m_AssetMapAssetBundle.Add(assetName, assetBundle);
				}
			}
		}
	}


	public AssetBundle GetAssetBundleFormName(string assetName) {
		if (!m_AssetMapAssetBundle.ContainsKey(assetName))
			return null;

		return m_AssetMapAssetBundle[assetName];
	}

	public Object LoadAsset(string assetName) {
		AssetBundle assetBundle = GetAssetBundleFormName(assetName);
		if (assetBundle != null) {
			int pos = assetName.IndexOf("/");
			return assetBundle.LoadAsset(assetName.Substring(pos+1));
		} else {
			return Resources.Load(assetName);
		}
	}

}