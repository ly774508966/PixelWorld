using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelUpdate : MonoBehaviour {

	Text text_msg;
	void Awake() {
		text_msg = transform.Find("Text msg").GetComponent<Text>();

	}


	bool bDownloading = false;

	void Start() {
		text_msg.text = "检查更新";

		UpdateManager.GetInstance().RequestVersion(delegate (WWW www){
			
			bool bNeedUpdate = UpdateManager.GetInstance().CompareVersion(www.text);

			// download resources
			if (bNeedUpdate) {
				string[] files = UpdateManager.GetInstance().UpdateFiles;
				int count = 1;
				AssetBundleManager.GetInstance().SetDownloadCallback(delegate {
					text_msg.text = string.Format("更新资源({0}/{1})", count++, files.Length);
				});
				for(int i = 0; i < files.Length; i ++) {
					AssetBundleManager.GetInstance().AddLoadAssetBundle(files[i]);
				}
				bDownloading = true;
			} else {
				// load local assetbundle
				Dictionary<string, string> localfiles = UpdateManager.GetInstance().LocalFiles;
				foreach(string file in localfiles.Keys) {
					AssetBundleManager.GetInstance().LoadAssetBundleLocal(file);
				}

				bDownloading = true;
			}
		});
	}

	void Update() {
		
		if (bDownloading) {
			if (AssetBundleManager.GetInstance().GetDownloadingWWWNum() == 0 && 
			AssetBundleManager.GetInstance().GetToLoadAssetBundleNum() == 0 ) {
				OnDownloadFinish();
				bDownloading = false;
			}
		}
	}


	// 更新检查完成
	void OnDownloadFinish() {

		ResourceManager.GetInstance().Init();

		LanguageManager.GetInstance().Init();

		UpdateManager.GetInstance().UpdateVersion();

		// 初始化lua engine
		LuaManager luaManager = LuaManager.GetInstance(true);
		luaManager.InitStart();
		luaManager.DoFile("Game");
		Util.CallMethod("Game", "OnInitOK");
	}
}
