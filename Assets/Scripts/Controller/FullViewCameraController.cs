/// <summary>
/// Mouse orbit.
/// This script use to control a main camera
/// </summary>

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class FullViewCameraController : MonoBehaviour {
	
	public Transform target; //a target look at
	public float height;
	public float distance;
	public float xSpeed; //speed pan x
	public float ySpeed; //speed pan y
	public float yMinLimit; //y min limit
	public float yMaxLimit; //y max limit
	public bool protectFromWallClip = true;
	public LayerMask clipLayer;

	//Private variable
	private float m_LookAngle;	// y rotation
	private float m_TiltAngle;

	// Use this for initialization
	void Start () {

		//Warning when not found target
		if(target == null)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			
			if(player == null)
			{
				Debug.LogWarning("Don't found player tag please change player tag to Player");	
				return;
			}

			target = player.transform;
		}
		

		//Setup Pos
		Vector3 angles = transform.eulerAngles;
		m_TiltAngle= angles.x;
		m_LookAngle = angles.y;

#if !MOBILE_INPUT
	Cursor.lockState = CursorLockMode.Locked;
#endif

	}

 
	void LateUpdate () {
		RotateCamera();
	}
	
	//Roate camera method
	void RotateCamera()
	{
		if (target == null) return;

		var x = CrossPlatformInputManager.GetAxis("Mouse X");
		var y = CrossPlatformInputManager.GetAxis("Mouse Y");
	

		m_LookAngle += x * xSpeed;

		m_TiltAngle -= y * ySpeed;
	
		m_TiltAngle = ClampAngle(m_TiltAngle, yMinLimit, yMaxLimit);

		Quaternion rotation = Quaternion.Euler(m_TiltAngle, m_LookAngle, 0);

		Vector3 targetPos = target.position + new Vector3(0, height, 0);

		Vector3 calPos = new Vector3(0, 0, -distance);

		Vector3 position = rotation * calPos + targetPos;

		transform.rotation = rotation;
		transform.position = Vector3.Lerp(transform.position, position, Time.time);

		// clip from walls
		if (protectFromWallClip) {
			Vector3 dir = (targetPos - transform.position).normalized;
			Vector3 end = targetPos + transform.eulerAngles.y * dir;

			Debug.DrawLine(position, targetPos, Color.red);

			RaycastHit hit;
			if (Physics.Linecast(targetPos, position, out hit, clipLayer)) {
				//Debug.Log(hit.collider.name);
				transform.position = hit.point;
			}
		}
	}

	float ClampAngle(float angle, float min, float max) {
		if (angle < -360) {
			angle += 360;
		} else if (angle > 360) {
			angle -= 360;
		}

		return Mathf.Clamp(angle, min, max);
	}

}
