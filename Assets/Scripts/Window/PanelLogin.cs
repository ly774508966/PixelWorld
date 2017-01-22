using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelLogin : MonoBehaviour {

	Text text_msg, text_equips, text_items, text_skills;
	void Awake() {
		text_msg = transform.Find("Text").GetComponent<Text>();
		text_equips = transform.Find("Text equips").GetComponent<Text>();
		text_items = transform.Find("Text items").GetComponent<Text>();
		text_skills = transform.Find("Text skills").GetComponent<Text>();
		text_msg.text = Application.persistentDataPath;
	}

	void Start() {
		AssetBundleManager.GetInstance().AddLoadAssetBundle("ui/panelalert");
		AssetBundleManager.GetInstance().AddLoadAssetBundle("box");
		AssetBundleManager.GetInstance().AddLoadAssetBundle("cfg");
	}

	bool bFirst = true;
	void Update() {
		
		if (bFirst) {
			if (AssetBundleManager.GetInstance().GetDownloadingWWWNum() == 0 && 
			AssetBundleManager.GetInstance().GetToLoadAssetBundleNum() == 0 ) {
				OnDownloadFinish();
				bFirst = false;
			}
		}
	}


	void OnDownloadFinish() {

		ResourceManager.GetInstance().Init();

		CfgManager.GetInstance().Init();

		AssetBundle assetBundle = AssetBundleManager.GetInstance().GetLoadedAssetBundle("box");
		if (assetBundle != null) {
			GameObject go =  (GameObject)Instantiate(assetBundle.LoadAsset("box", typeof(GameObject)));
			go.transform.localPosition = new Vector3(300, 0, 500);
			go =  (GameObject)Instantiate(assetBundle.LoadAsset("box", typeof(GameObject)));
			go.transform.localPosition = new Vector3(-300, 0, 500);
		}


		text_equips.text = CfgManager.GetInstance().cfg_equips;
		text_items.text = CfgManager.GetInstance().cfg_items;
		text_skills.text = CfgManager.GetInstance().cfg_skills;
	}



	public void OnBtnAlert() {
		GUIManager.GetInstance().ShowAlert("提示", "提示框测试");
	}
}
