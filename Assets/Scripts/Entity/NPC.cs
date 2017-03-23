using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public int ID;					// id
	public string Name {get; set;}		// name


	//动画组件
	protected Animator m_Animator;

	void Awake() {
		m_Animator = GetComponentInChildren<Animator>();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
