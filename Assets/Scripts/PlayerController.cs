using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public enum CharaterState {
	IDLE=0,
	IDLE2,
	RUN,
	JUMP,
	ATTACK1_1,
	ATTACK1_2,
	ATTACK1_3,
	ATTACK2,
	DEATH,
}

public class PlayerController : MonoBehaviour {
 
	//移动速度
	public float MoveSpeed=1.5F;
	//奔跑速度
	public float RunSpeed=4.5F;
	//旋转速度
	public float RotateSpeed=30;
	//重力
	public float Gravity=20;
	//动画组件
	private Animation mAnim;
	//速度
	private float mSpeed;

	// state
	private CharaterState mCharaterState;

	//角色控制器
	private CharacterController mController;
	 
	void Start ()
	{
		//获取角色控制器
		mController=GetComponent<CharacterController>();
		//获取动画组件
		mAnim=GetComponentInChildren<Animation>();
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

		if (mController.isGrounded) {
			if (bJump) {
				mDir.y = 20;
				// 水平
				mAnim.Play ("jump");
			} else if (isMoving) {
				transform.forward = mDir;
				mDir = mDir * RunSpeed;
				mAnim.Play ("run");
			} else {
				mAnim.Play ("idle");
			}
		} else {
			mDir.x = 0;
			mDir.z = 0;
		}

		//考虑重力因素
		//mDir=transform.TransformDirection(mDir);
		float y = mDir.y-Gravity *Time.fixedDeltaTime;
		mDir=new Vector3(mDir.x,y,mDir.z);
		mController.Move(mDir * Time.fixedDeltaTime);
	}
 
}