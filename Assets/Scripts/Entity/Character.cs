using UnityEngine;
using System.Collections;

public enum CharaterState {
	IDLE=0,
	IDLE2,
	RUN,
	JUMP,
	ATTACK,
	ATTACK1_1,
	ATTACK1_2,
	ATTACK1_3,
	ATTACK2,
	ATTACK3,
	HIT,
	DEATH,
}

public class Character : MonoBehaviour {

	public int ID {get; set;}			// uid
	public int ModelID {get; set;}		// model id
	public string Name {get; set;}		// name

	// attrs
	public int HP = 100;				// hp
	public int MP = 100;				// mp
	
	public float Speed = 4.0f;			// 移动速度
	public float DisSight = 10.0f;		// 可视范围
	public float DisAttack = 1.0f; 		// 攻击范围

	//动画组件
	protected Animator m_Animator;

	protected CharaterState m_CharacterState;
	public CharaterState CharcterState {get{return m_CharacterState;}}


	protected GameObject AttackBox;

	void Awake() {
		m_Animator = GetComponentInChildren<Animator>();

		AttackBox = transform.Find("AttackBox").gameObject;
		AttackBox.SetActive(false);

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void ActIdle() {
		m_CharacterState = CharaterState.IDLE;
		m_Animator.CrossFade ("idle", 0);
	}
	public virtual void ActRun() {
		m_CharacterState = CharaterState.RUN;
		m_Animator.CrossFade ("run", 0);
	}
	public virtual void ActAttack() {
		m_CharacterState = CharaterState.ATTACK;
		m_Animator.CrossFade ("attack", 0);
	}

	public virtual void ActHit() {
		m_CharacterState = CharaterState.HIT;
		m_Animator.SetBool("bHit", true);
		StartCoroutine(ResetValue("bHit"));
	}
	public virtual void ActDie() {
		m_CharacterState = CharaterState.DEATH;
		m_Animator.SetBool("bDie", true);
	}


	public IEnumerator ResetValue(string name)
	{
		yield return null;
		m_Animator.SetBool(name, false);
	}

	protected virtual void StartAttack() {
	}

	public void OnEventAttack(string param) {
		//Debug.LogFormat("OnEventAttack {0} {1}", ID, param);
		if (param == "start") {
			AttackBox.SetActive(true);

			StartAttack();
		} else {
			AttackBox.SetActive(false);
		}
	}
}
