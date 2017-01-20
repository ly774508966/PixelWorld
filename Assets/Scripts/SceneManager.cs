/// <summary>
/// Scene manager.
/// 场景管理
/// </summary>

using UnityEngine;
using System.Collections;

public enum SceneID : byte {
	Loading=0,
	Login,
	Create,
	World,		
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
		case SceneID.Loading:
			UnityEngine.SceneManagement.SceneManager.LoadScene(2);
			break;
		case SceneID.Login:
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
			break;
		case SceneID.Create:
			UnityEngine.SceneManagement.SceneManager.LoadScene(3);
			break;
		case SceneID.World:
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			break;
		default:
			Debug.LogError("error sceneid");
			break;
		}

		_CurrentSceneID = id;
	}
}
