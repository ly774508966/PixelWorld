using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelLogin : MonoBehaviour {

	Text text_msg;
	GameObject btn_ok;
	void Awake() {
		text_msg = transform.Find("Text msg").GetComponent<Text>();
		btn_ok = transform.Find("Button OK").gameObject;

	}


	bool bDownloading = false;

	void Start() {
		text_msg.text = "检查更新";
		btn_ok.SetActive(false);

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
				RefreshPanel();
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
		btn_ok.SetActive(true);

		AssetBundle assetBundle = AssetBundleManager.GetInstance().GetLoadedAssetBundle("box");
		if (assetBundle != null) {
			GameObject go =  (GameObject)Instantiate(assetBundle.LoadAsset("box", typeof(GameObject)));
			go.transform.localPosition = new Vector3(300, 0, 500);
			go =  (GameObject)Instantiate(assetBundle.LoadAsset("box", typeof(GameObject)));
			go.transform.localPosition = new Vector3(-300, 0, 500);
		}
	}


	public void OnBtnAlert() {
		GUIManager.GetInstance().ShowAlert("提示", "提示框测试");
	}
}
