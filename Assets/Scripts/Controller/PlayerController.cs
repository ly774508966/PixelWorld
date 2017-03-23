using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
 
	//重力
	public float Gravity=10;
	//速度
	private float mSpeed;

	//
	private float yMove = 0;	// 垂直速度
	private Vector3 move = Vector3.zero; 

	private int m_AttackIdx = 0;

	//角色控制器
	private CharacterController m_Controller;

	//角色控制器
	private Animator m_Animator;

	private Player m_Player;

	void Start ()
	{
		//获取角色控制器
		m_Controller=GetComponent<CharacterController>();
		m_Animator = GetComponentInChildren<Animator>();
		//
		m_Player=GetComponentInChildren<Player>();
	}

	void Update () {
		//只有处于正常状态时玩家可以行动
		MoveManager ();

	}
	 
	//移动管理
	void MoveManager()
	{
		//移动方向
		Vector3 mDir=Vector3.zero;

		// Read input
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");


		bool bJump = CrossPlatformInputManager.GetButtonDown ("Jump");
		bool bAttack = CrossPlatformInputManager.GetButtonDown ("Fire1");
		bool bSkill1 = CrossPlatformInputManager.GetButtonDown ("Fire2");
		bool bSkill2 = CrossPlatformInputManager.GetButtonDown ("Fire3");
		if (bJump) {
			Debug.LogFormat("bJump {0}", bJump);
		}
		bool isMoving = false;

		if (Mathf.Abs (horizontal) > 0.01f || Mathf.Abs (vertical) > 0.01f) {
			// move
			isMoving = true;
			mDir.x = horizontal;
			mDir.z = vertical;	

			// normalize input if it exceeds 1 in combined length:
			if (mDir.sqrMagnitude > 1) {
				mDir.Normalize();
			}

			// camera 
			mDir = Camera.main.transform.forward * mDir.z + Camera.main.transform.right * mDir.x;
			mDir.y = 0;
		} else {
			// move stop
			isMoving = false;
		}


		if (m_Controller.isGrounded == false) {
			bJump = false;
			bAttack = false;
			bSkill1 = false;
			bSkill2 = false;
		}

		if (bJump) {
			yMove = 5;
			Debug.LogFormat("bJump {0}", bJump);
		}

		m_Animator.SetBool("isMoving", isMoving);
		m_Animator.SetBool("isGrounded", m_Controller.isGrounded);

		AnimatorStateInfo cur = m_Animator.GetCurrentAnimatorStateInfo(0);
		if (m_Animator.IsInTransition(0)) {
			// 融合时
			AnimatorStateInfo next = m_Animator.GetNextAnimatorStateInfo(0);
		} else {
			// 不融合时
			if (cur.IsName("run") && isMoving) {
				transform.forward = mDir;
				move.x = mDir.x * m_Player.Speed;
				move.z = mDir.z * m_Player.Speed;
				m_AttackIdx = 0;
				m_Animator.SetBool("bJump", bJump);
				if (bAttack) {
					m_AttackIdx = 1;
				}
				m_Animator.SetInteger("AttackIdx", m_AttackIdx);
				m_Animator.SetBool("bSkill1", bSkill1);
				m_Animator.SetBool("bHit", false);
				//Debug.Log("run");
			} else if (cur.IsName("idle")) {
				move.x = 0;
				move.z = 0;
				m_Animator.SetBool("bJump", bJump);
				m_AttackIdx = 0;
				if (bAttack) {
					m_AttackIdx = 1;
				}
				m_Animator.SetInteger("AttackIdx", m_AttackIdx);
				m_Animator.SetBool("bSkill1", bSkill1);
				//Debug.Log("idle");
			} else if (cur.IsName("jump")) {
				if (yMove > 0 && cur.normalizedTime < 0.5f) {	// 开始jump
					move.y = yMove;		// 向上初速度
					yMove = 0;
				}
				Debug.Log("jump " + cur.normalizedTime);
			} else if (cur.IsName("attack1_1")) {
				if (isMoving)  transform.forward = mDir;
				move.x = 0;
				move.z = 0;
				if (bAttack && m_AttackIdx == 1) {
					m_AttackIdx ++;
					m_Animator.SetInteger("AttackIdx", m_AttackIdx);
				}
			} else if (cur.IsName("attack1_2")) {
				if (isMoving)  transform.forward = mDir;
				move.x = 0;
				move.z = 0;
				if (bAttack && m_AttackIdx == 2) {
					m_AttackIdx ++;
					m_Animator.SetInteger("AttackIdx", m_AttackIdx);
				}
			} else if (cur.IsName("attack1_3")) {
				if (isMoving)  transform.forward = mDir;
				move.x = 0;
				move.z = 0;
			} else if (cur.IsName("hit")) {
				move.x = 0;
				move.z = 0;
				m_Animator.SetBool("bHit", false);
			} else if (cur.IsName("skill1")) {
				move.x = 0;
				move.z = 0;
				m_Animator.SetBool("bSkill1", false);
			}
		}

		/*
		if (m_Controller.isGrounded) {

			if (m_Player.CharcterState == CharaterState.IDLE || m_Player.CharcterState == CharaterState.RUN) {
				move = mDir * RunSpeed;
			} else {
				// no move 
				isMoving = false;
			}
			if (bAttack) {
				move.x = 0;
				move.z = 0;
				m_Player.ActAttack1();
			} else if (bJump) {
				move.y = 4;		// 向上初速度
				m_Player.ActJump();
			} else if (isMoving) {
				transform.forward = mDir;
				if (m_Player.CharcterState != CharaterState.RUN) {
					m_Player.ActRun();
				}
			} else {
				move.x = 0;
				move.z = 0;
				// 移动停止 or 跳跃着地
				if (m_Player.CharcterState == CharaterState.RUN || m_Player.CharcterState == CharaterState.JUMP) m_Player.ActIdle();
			}
		} else {
			// in air
		}
		*/

		if (!m_Controller.isGrounded) {
			// 重力下降
			move.y -= Gravity *Time.deltaTime;
		}

		CollisionFlags flags = m_Controller.Move(move * Time.deltaTime);

		//Debug.Log(m_Controller.isGrounded);
	}
 
}