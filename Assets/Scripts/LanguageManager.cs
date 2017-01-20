/// <summary>
/// DataManager
/// 管理所有配置
/// </summary>
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;


public class LanguageManager {
	private static LanguageManager _instance;
	public static LanguageManager GetInstance() {
		if (_instance == null) _instance = new LanguageManager();
		return _instance;
	}
	private static bool s_init = false;

	// shop
	private Dictionary<string, string> values = new Dictionary<string, string>();

	public string GetValue(string key) {
		if (values.ContainsKey(key)) {
			return values[key];
		} else {
			return "";
		}
	}

	public void Init() {
		if (s_init) return;

		// block cfg
		TextAsset asset = Resources.Load("Localization/zh-cn") as TextAsset;
		string[] lines = asset.text.Split(new char[]{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 1; i < lines.Length; i ++) {
			lines[i].TrimStart();
			if (lines[i][0] == '#') continue;	// 注释

			string[] strs = lines[i].Split('=');
			if (strs.Length == 2 ) {
				values[strs[0].Trim()] = strs[1].TrimStart();
			} else {
				Debug.LogError("error format!");
				continue;
			}
		}

		s_init = true;
	}


}
