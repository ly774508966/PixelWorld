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
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			Player player = collider.transform.parent.GetComponent<Player>();
			Debug.Log("player " + player.ID);
			ActHit();
			BattleManager.GetInstance ().EnemyHit (ID, player.ID);
		}  
	}
	void OnTriggerExit(Collider collider)  {  
		//Debug.Log("OnTriggerExit");  
	}  
}
