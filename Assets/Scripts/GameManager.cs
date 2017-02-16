using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Awake () {
		DontDestroyOnLoad(gameObject);

		instance = this;

		// 初始化lua engine
		LuaManager luaManager = LuaManager.GetInstance();
		luaManager.InitStart();
		luaManager.DoFile("Game");
		Util.CallMethod("Game", "OnInitOK");
	}
}