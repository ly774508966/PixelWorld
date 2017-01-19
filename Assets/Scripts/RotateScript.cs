using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class RotateScript : MonoBehaviour {

	void Awake() {
		
	}

	void Update() {
		transform.Rotate(new Vector3(0, 1, 0), Space.World);

	}
}