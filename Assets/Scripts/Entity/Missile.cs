//
//	发射物
//
using UnityEngine;
using System.Collections;

public enum MissileType {
	ONCE,
	LONG,

}

public class Missile : MonoBehaviour {

	public int ID {get; set;}			// uid
	public int OwnID {get; set;}		// own id

	public MissileType Type;
	public float Speed = 4.0f;			// 移动速度
	public float DisAttack = 1.0f; 		// 攻击范围


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
