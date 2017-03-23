using UnityEngine;
using System.Collections;

public class Boss : Character {

	protected GameObject SkillBox;

	// Use this for initialization
	void Start () {
		
		SkillBox = transform.Find("SkillBox").gameObject;
		SkillBox.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	}


	public void OnEventSkill1(string param) {
		//Debug.LogFormat("OnEventAttack {0} {1}", ID, param);
		if (param == "start") {
			SkillBox.SetActive(true);

			StartSkill1();
		} else {
			SkillBox.SetActive(false);
		}
	}

	protected override void StartAttack ()
	{
		base.StartAttack ();
	}

	protected void StartSkill1 ()
	{
		
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
