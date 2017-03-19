using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	private static BattleManager instance;
	public static BattleManager GetInstance() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
	
		if (instance == null) {
			instance = main.AddComponent<BattleManager>();
		}
		return instance;
	}

	public object[] CallMethod(string func, params object[] args) {
		return Util.CallMethod("battle", func, args);
	}

	public void PlayerHit(int id, int attackid){
		CallMethod("player_hit", id, attackid);
	}

	public void EnemyHit(int id, int attackid){
		CallMethod("enemy_hit", id, attackid);
	}
}