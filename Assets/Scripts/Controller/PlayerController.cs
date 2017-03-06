using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
 
	//移动速度
	public float MoveSpeed=1.5F;
	//奔跑速度
	public float RunSpeed=4.5F;
	//旋转速度
	public float RotateSpeed=30;
	//重力
	public float Gravity=20;
	//速度
	private float mSpeed;

	//
	private float yMove = 0;	// 垂直速度
	private Vector3 move = Vector3.zero; 


	//角色控制器
	private CharacterController m_Controller;

	private Player m_Player;

	void Start ()
	{
		//获取角色控制器
		m_Controller=GetComponent<CharacterController>();
		//
		m_Player=GetComponentInChildren<Player>();
	}
	 
	void Update ()
	{
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
		} else {
			// move stop
			isMoving = false;
		}

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
				move.y = 6;		// 向上初速度
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

		// 重力下降
		move.y -= Gravity *Time.fixedDeltaTime;
		m_Controller.Move(move * Time.fixedDeltaTime);
	}
 
}