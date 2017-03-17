using UnityEngine;
using System.Collections;

public class Monster : Character {

	private GameObject HitBox;

	void Awake() {
		HitBox = transform.Find("Weapon").gameObject;
		HitBox.SetActive(false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void OnEventAttack(string param) {
		Debug.LogFormat("OnEventAttack {0} {1}", ID, param);
		if (param == "start") {
			HitBox.SetActive(true);
		} else {
			HitBox.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider collider)   { 
		Debug.Log("OnTriggerEnter");
		if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
			Player player = collider.transform.parent.GetComponent<Player>();
			Debug.Log("player " + player.ID);
			ActHit();
		}  
	}
	void OnTriggerExit(Collider collider)  {  
		Debug.Log("OnTriggerExit");  
	}  
}
