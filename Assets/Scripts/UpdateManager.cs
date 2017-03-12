using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class UpdateManager : MonoBehaviour {

	private static UpdateManager instance;
	public static UpdateManager GetInstance() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
	
		if (instance == null) {
			instance = main.AddComponent<UpdateManager>();
		}
		return instance;
	}


	List<string> m_UpdateFiles = new List<string>();
	public string[] UpdateFiles {get {return m_UpdateFiles.ToArray();}}
	public Dictionary<string, string> LocalFiles {get {return m_LocalFiles;}}

	// version files
	string m_ServerVersionFile;
	string m_ServerVersion;
	string m_LocalVersion;
	Dictionary<string, string> m_ServerFiles = new Dictionary<string, string>();
	Dictionary<string, string> m_LocalFiles = new Dictionary<string, string>();

	public void Init() {
		Debug.Log("UpdateManager:Init");
	}

	// download version
	public void RequestVersion(HandleDownloadFinish handler) {		
		StartCoroutine( Download(delegate (WWW www){
			handler(www);
		}
		));
	}

	IEnumerator Download(HandleDownloadFinish handler) {
		string url = AssetBundleManager.GetInstance().BaseDownloaindURL + "resourcelist.txt";

		WWW www = new WWW(url);

		yield return www;

		handler(www);

		www.Dispose();
	}

	// compare local version 
	public bool CompareVersion(string versionFile) {
		m_ServerVersionFile = versionFile;
		m_LocalFiles.Clear();
		m_ServerFiles.Clear();
		m_UpdateFiles.Clear();

		string[] lines = versionFile.Split(new char[]{'\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		m_ServerVersion = lines[0];
		m_ServerVersion = m_ServerVersion.Split(' ')[1];
		for(int i = 1; i < lines.Length; i ++) {
			string[] strs = lines[i].Split(' ');
			if (strs.Length != 3) {
				Debug.Log("error format!");
			} else {
				m_ServerFiles.Add(strs[0], strs[1]);
			}
		}

		// local version files
		string filename = Path.Combine(Application.persistentDataPath, "resourcelist.txt");
		if (!File.Exists(filename)) {
			TextAsset asset = Resources.Load("version", typeof(TextAsset)) as TextAsset;
			lines = asset.text.Split(new char[]{'\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);
			m_LocalVersion = lines[0];
			m_LocalVersion = m_LocalVersion.Split(' ')[1];
		} else {
			FileStream fs = new FileStream(filename, FileMode.Open);
			StreamReader reader = new StreamReader(fs);
			m_LocalVersion = reader.ReadLine();
			m_LocalVersion = m_LocalVersion.Split(' ')[1];
			string line;
			while((line = reader.ReadLine()) != null) {
				string[] strs = line.Split(' ');
				if (strs.Length != 3) {
					Debug.Log("error format!");
				} else {
					m_LocalFiles.Add(strs[0], strs[1]);
				}
			}
			reader.Close();
			fs.Close();
		}

		// compare
		foreach(string file in m_ServerFiles.Keys) {
			string crc = m_ServerFiles[file];
			if (!m_LocalFiles.ContainsKey(file) || crc.CompareTo(m_LocalFiles[file]) != 0) {
				m_UpdateFiles.Add(file);
			}
		}

		if (m_LocalVersion.CompareTo(m_ServerVersion) != 0 || m_UpdateFiles.Count > 0) {
			return true;
		} else {
			return false;
		}
	}


	public void UpdateVersion() {
		string filename = Path.Combine(Application.persistentDataPath, "resourcelist.txt");
		FileStream fs = new FileStream(filename, FileMode.Create);
		StreamWriter writer = new StreamWriter(fs);
		writer.Write(m_ServerVersionFile);
		writer.Flush();
		fs.Flush();
		writer.Close();
		fs.Close();
	}

}