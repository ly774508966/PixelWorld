using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelMenu : MonoBehaviour {

	void Awake() {

		GameObject obj = ResourceManager.GetInstance().LoadAsset("prefabs/box") as GameObject;
		GameObject go =  (GameObject)Instantiate(obj);
		go.transform.localPosition = new Vector3(200, 0, 500);
		go =  (GameObject)Instantiate(obj);
		go.transform.localPosition = new Vector3(-200, 0, 500);
	
	}

	public void OnBtnAlert() {
		GUIManager.GetInstance().ShowAlert("提示", "提示框测试");
	}
}
