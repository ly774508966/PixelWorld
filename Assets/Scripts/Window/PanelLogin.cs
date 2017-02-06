using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelLogin : MonoBehaviour {

	Text text_msg;
	GameObject btn_login;
	void Awake() {
		text_msg = transform.Find("Text msg").GetComponent<Text>();
		btn_login = transform.Find("Button Login").gameObject;

	}


	bool bDownloading = false;

	void Start() {
		text_msg.text = "检查更新";
		btn_login.SetActive(false);

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


	void OnDownloadFinish() {

		ResourceManager.GetInstance().Init();

		CfgManager.GetInstance().Init();

		UpdateManager.GetInstance().UpdateVersion();

		RefreshPanel();
	}


	void RefreshPanel() {

		text_msg.text = "已是最新版本";
		btn_login.SetActive(true);

	}


	public void OnBtnLogin() {
		SceneManager.GetInstance().GotoScene(SceneID.Main);
	}
}
