using UnityEngine;
using System.Collections;

public class LockViewCameraController : MonoBehaviour {

	[SerializeField] protected Transform m_Target;            // The target object to follow
	[SerializeField] private float m_MoveSpeed = 10f;           // How fast the rig will move to keep up with the target's position.
	[SerializeField] private Vector3 m_Offset = new Vector3(0, 5, -10);                // offset
    [SerializeField] private bool m_IgnoreY = true;                // ignore y move

	public void SetTarget(Transform target) {
		m_Target = target;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (m_Target == null) return;
		// Move the rig towards target position.
		if (m_IgnoreY) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(m_Target.position.x, 0, m_Target.position.z)+m_Offset, Time.deltaTime*m_MoveSpeed);
		} else {
			transform.position = Vector3.Lerp(transform.position, m_Target.position+m_Offset, Time.deltaTime*m_MoveSpeed);
		}
	}
}
