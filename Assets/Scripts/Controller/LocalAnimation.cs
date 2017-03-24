using UnityEngine;
 
[RequireComponent(typeof(Animation))]
public class LocalAnimation : MonoBehaviour
{
	Vector3 localPos;
	bool wasPlaying;

	Animation ani;

	void Awake()
	{
		localPos = transform.localPosition;
		ani = GetComponent<Animation>();
		wasPlaying = false;
	}

	void LateUpdate()
	{
		if (!ani.isPlaying && !wasPlaying) return;

		transform.localPosition += localPos;
		
		wasPlaying = ani.isPlaying;
	}
}