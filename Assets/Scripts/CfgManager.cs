using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CfgManager : MonoBehaviour {

	private static CfgManager instance;
	public static CfgManager GetInstance() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
	
		if (instance == null) {
			instance = main.AddComponent<CfgManager>();
		}
		return instance;
	}

	public string cfg_equips;
	public string cfg_items;
	public string cfg_skills;

	public void Init() {
		Debug.Log("CfgManager:Init");

		cfg_equips = ResourceManager.GetInstance().LoadAsset("cfg/equips").ToString();
		cfg_items = ResourceManager.GetInstance().LoadAsset("cfg/items").ToString();
		cfg_skills = ResourceManager.GetInstance().LoadAsset("cfg/skills").ToString();
	}

}