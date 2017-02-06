/// <summary>
/// Scene manager.
/// 场景管理
/// </summary>

using UnityEngine;
using System.Collections;

public enum SceneID : byte {
	Login,
	Main,		
}

public class SceneManager {
	private static SceneManager _instance;
	public static SceneManager GetInstance() {
		if (_instance == null) _instance = new SceneManager();
		return _instance;
	}

	private SceneID _CurrentSceneID;
	public SceneID CurrentSceneID {get {return _CurrentSceneID;}}

	public void GotoScene(SceneID id) {
		switch(id) {
		case SceneID.Login:
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			break;
		case SceneID.Main:
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			break;
		default:
			Debug.LogError("error sceneid");
			break;
		}

		_CurrentSceneID = id;
	}
}
