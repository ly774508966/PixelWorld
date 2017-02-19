using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Awake () {
		DontDestroyOnLoad(gameObject);

		instance = this;

	}
}