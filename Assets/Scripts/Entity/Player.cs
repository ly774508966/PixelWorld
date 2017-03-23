using UnityEngine;
using System.Collections;

public class Player : Character {

	//角色控制器
	private CharacterController m_Controller;

	private int m_AttackIdx = 1;


	protected GameObject SkillBox;

	// Use this for initialization
	void Start () {

		m_Controller=GetComponent<CharacterController>();

		SkillBox = transform.Find("SkillBox").gameObject;
		SkillBox.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public virtual void ActJump() {
		Debug.Log("ActJump");
		m_CharacterState = CharaterState.JUMP;
		m_Animator.CrossFade("jump", 0);
	}

	public virtual void ActAttack1() {
		Debug.Log("ActAttack1");
		switch(m_CharacterState) {
		case CharaterState.ATTACK1_1:
			m_AttackIdx = 2;
			break;
		case CharaterState.ATTACK1_2:
			m_AttackIdx = 3;
			break;
		case CharaterState.ATTACK1_3:
			m_AttackIdx = 1;
			break;
		default:
			m_AttackIdx = 1;
			m_CharacterState = CharaterState.ATTACK1_1;
			m_Animator.CrossFade("attack1_1", 0);
			break;
		}
	}

	private void ActAttack1_2() {
		m_CharacterState = CharaterState.ATTACK1_2;
		m_Animator.CrossFade ("attack1_2", 0);
	}
	private void ActAttack1_3() {
		m_CharacterState = CharaterState.ATTACK1_3;
		m_Animator.CrossFade ("attack1_3", 0);
	}

	public void OnAnimationComplete() {
		Debug.LogFormat("amation state:{0} complete", m_CharacterState);

		if (m_CharacterState == CharaterState.ATTACK1_1 && m_AttackIdx > 1) {
			m_Controller.Move(transform.forward*0.5f);
			ActAttack1_2();
		} else if (m_CharacterState == CharaterState.ATTACK1_2 && m_AttackIdx > 2) {
			m_Controller.Move(transform.forward*0.5f);
			ActAttack1_3();
		} else {
			ActIdle();
		}
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

		bool hasEnemy = CharacterManager.GetInstance().CheckEnemyInArea(AttackBox.transform.position, DisAttack);
		Debug.Log("hasEnemy " + hasEnemy);

		if (hasEnemy == false) {
			// auto-rotate
			float distance = 0;
			Monster monster = CharacterManager.GetInstance().FindNearestEnemy(transform.position, out distance);
			if (distance < DisAttack*10) {
				Vector3 offset = monster.transform.position - transform.position;
				offset.y = 0;
				transform.forward = offset.normalized;
			}
		}
	}

	protected void StartSkill1 ()
	{
		bool hasEnemy = CharacterManager.GetInstance().CheckEnemyInArea(AttackBox.transform.position, DisAttack);
		Debug.Log("hasEnemy " + hasEnemy);

		if (hasEnemy == false) {
			// auto-rotate
			float distance = 0;
			Monster monster = CharacterManager.GetInstance().FindNearestEnemy(transform.position, out distance);
			if (distance < DisAttack*10) {
				Vector3 offset = monster.transform.position - transform.position;
				offset.y = 0;
				transform.forward = offset.normalized;
			}
		}

		// mis
	}

	void OnTriggerEnter(Collider collider)   { 
		string tag = collider.gameObject.tag;
		Debug.Log("OnTriggerEnter " + tag);  
		if ( tag == "Enemy") {
			Monster monster = collider.transform.parent.GetComponent<Monster>();
			Debug.Log("Enemy " + monster.ID);
			ActHit();
			BattleManager.GetInstance ().PlayerHit (ID, monster.ID);
		} else if (tag == "NPC") {

			NPC npc = collider.transform.GetComponent<NPC>();

			BattleManager.GetInstance ().PlayerEnterNpc (ID, npc.ID);
		}
	}  
	void OnTriggerExit(Collider collider)  {  
		//Debug.Log("OnTriggerExit");  
	}
}
