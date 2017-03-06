using UnityEngine;
using System.Collections;

public class Player : Character {

	// Use this for initialization
	void Start () {

		AnimationClip clip = m_Animation.GetClip("attack1_1");
		AnimationEvent evt = new AnimationEvent();
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
		m_CharacterState = CharaterState.ATTACK1_1;
		m_Animation.Play ("attack1_1");
	}

	public void OnAnimationComplete() {
		Debug.LogFormat("amation state:{0} complete", m_CharacterState);
		ActIdle();
	}
}
