using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	private static CharacterManager instance;
	public static CharacterManager GetInstance() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
	
		if (instance == null) {
			instance = main.AddComponent<CharacterManager>();
		}
		return instance;
	}


	private List<Character> m_Characters = new List<Character>();


	public Player AddPlayer() {
		Object prefab = ResourceManager.GetInstance().LoadAsset("Prefabs/Character/king");
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		Player player = go.GetComponent<Player>();
		m_Characters.Add(player);
		return player;
	}  

	public Monster AddMonster(int id) {
		Object prefab = ResourceManager.GetInstance().LoadAsset("Prefabs/Monster/"+id);
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		Monster monster = go.GetComponent<Monster>();
		m_Characters.Add(monster);
		return monster;
	}  

	public bool RemoveCharacter(Character character) {
		Destroy(character.gameObject);
		return m_Characters.Remove(character);
	}
}