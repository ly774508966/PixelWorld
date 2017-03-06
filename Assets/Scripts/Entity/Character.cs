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
	//动画组件
	protected Animation m_Animation;

	protected CharaterState m_CharacterState;
	public CharaterState CharcterState {get{return m_CharacterState;}}

	void Awake() {
		m_Animation = GetComponentInChildren<Animation>();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void ActIdle() {
		m_CharacterState = CharaterState.IDLE;
		m_Animation.Play ("idle");
	}
	public virtual void ActRun() {
		m_CharacterState = CharaterState.RUN;
		m_Animation.Play ("run");
	}
	public virtual void ActAttack() {
		m_CharacterState = CharaterState.ATTACK;
		m_Animation.Play ("attack");
	}
	public virtual void ActDeath() {
		m_CharacterState = CharaterState.DEATH;
		m_Animation.Play ("death");
	}
	public virtual void ActHit() {
		m_CharacterState = CharaterState.HIT;
		m_Animation.Play ("hit");
	}
}
