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
	public float Life = 10.0f;

	private Rigidbody rigidbody;
	private float timer;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > Life) {
			Destroy(gameObject);
		}
		rigidbody.velocity = transform.forward * Speed;
	}

	public void OnCollision() {
		if (Type == MissileType.ONCE) {
			Destroy(gameObject);
		}
	}
}
