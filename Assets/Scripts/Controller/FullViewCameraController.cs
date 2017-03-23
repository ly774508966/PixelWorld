/// <summary>
/// Mouse orbit.
/// This script use to control a main camera
/// </summary>

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class FullViewCameraController : MonoBehaviour {
	
	public GameObject target; //a target look at
	public float distance;
	public float xSpeed; //speed pan x
	public float ySpeed; //speed pan y
	public float yMinLimit; //y min limit
	public float yMaxLimit; //y max limit

	//Private variable
	private float m_LookAngle;	// y rotation
	private float m_TiltAngle;

	// Use this for initialization
	void Start () {

		//Warning when not found target
		if(target == null)
		{
			target = GameObject.FindGameObjectWithTag("Player");
			
			if(target == null)
			{
				Debug.LogWarning("Don't found player tag please change player tag to Player");	
			}
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

		Vector3 calPos = new Vector3(0, 0, -distance);

		Vector3 position = rotation * calPos + target.transform.position;

		transform.rotation = rotation;
		transform.position = position;
	
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
