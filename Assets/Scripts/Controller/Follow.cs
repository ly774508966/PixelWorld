using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform target;
	public Vector3 offset;

	private RectTransform rectTransform;

	void Start () {
		rectTransform = GetComponent<RectTransform>();
	}

	void Update () {

		if (target == null || rectTransform == null) {
		    return;
		}

		Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(target.position + offset);

		//定义一个接收转换为 UI  2D 世界坐标的变量
		Vector3 followPosition;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, targetScreenPosition, null, out followPosition)) {
			transform.position = followPosition;
		}
	}
}