using UnityEngine;
using System.Collections;

public class Player : Character {

	//角色控制器
	private CharacterController m_Controller;

	private int m_AttackIdx = 1;

	// Use this for initialization
	void Start () {

		m_Controller=GetComponent<CharacterController>();

		AnimationClip clip = m_Animation.GetClip("attack1_1");
		AnimationEvent evt = new AnimationEvent();
		evt.functionName = "OnAnimationComplete";
		evt.time = clip.length;
		clip.AddEvent(evt);

		clip = m_Animation.GetClip("attack1_2");
		evt = new AnimationEvent();
		evt.functionName = "OnAnimationComplete";
		evt.time = clip.length;
		clip.AddEvent(evt);

		clip = m_Animation.GetClip("attack1_3");
		evt = new AnimationEvent();
		evt.functionName = "OnAnimationComplete";
		evt.time = clip.length;
		clip.AddEvent(evt);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void ActJump() {
		m_CharacterState = CharaterState.JUMP;
		m_Animation.Play ("jump");
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
			m_Animation["attack1_1"].speed = 1.5f;
			m_Animation.Play ("attack1_1");
			break;
		}
	}

	private void ActAttack1_2() {
		m_CharacterState = CharaterState.ATTACK1_2;
		m_Animation["attack1_2"].speed = 1.5f;
		m_Animation.Play ("attack1_2");
	}
	private void ActAttack1_3() {
		m_CharacterState = CharaterState.ATTACK1_3;
		m_Animation["attack1_3"].speed = 1.5f;
		m_Animation.Play ("attack1_3");
	}

	public void OnAnimationComplete() {
		Debug.LogFormat("amation state:{0} complete", m_CharacterState);

		if (m_CharacterState == CharaterState.ATTACK1_1 && m_AttackIdx > 1) {
			m_Controller.Move(transform.forward*0.8f);
			ActAttack1_2();
		} else if (m_CharacterState == CharaterState.ATTACK1_2 && m_AttackIdx > 2) {
			m_Controller.Move(transform.forward*0.8f);
			ActAttack1_3();
		} else {
			ActIdle();
		}
	}
}
