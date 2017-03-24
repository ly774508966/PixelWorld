using UnityEngine;
using System.Collections;

public class Monster : Character {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider collider)   { 
		Debug.Log("OnTriggerEnter");
		string tag = collider.gameObject.tag;
		if (tag == "Player") {
			Player player = collider.transform.parent.GetComponent<Player>();
			Debug.Log("player " + player.ID);
			ActHit();
			BattleManager.GetInstance ().EnemyHit (ID, player.ID);
		} else if (tag == "PlayerMissile") {
			Missile missile = collider.transform.GetComponent<Missile>();
			missile.OnCollision();
			Debug.Log("missile " + missile.ID);
			ActHit();
			BattleManager.GetInstance ().EnemyHit (ID, missile.ID);
		}
	}
	void OnTriggerExit(Collider collider)  {  
		//Debug.Log("OnTriggerExit");  
	}  
}
