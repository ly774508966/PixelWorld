using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIManager : MonoBehaviour {

	private static GUIManager _instance;
	public static GUIManager GetInstance() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
	
		if (_instance == null) {
			_instance = main.AddComponent<GUIManager>();
		}
		return _instance;
	}

	enum LayerPriority {
		Normal=0,
		Top,
	}

	private class UIView {
		public string name;
		public RectTransform rt;
	}

	private RectTransform	m_Root;

	private Dictionary<string, string> m_PanelMap = new Dictionary<string, string>();
	private Dictionary<string, LayerPriority> m_PanelLayerMap = new Dictionary<string, LayerPriority>();
	private List<UIView>		m_Stack = new List<UIView>();


	void Awake () {
		Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		m_Root = canvas.GetComponent<RectTransform>();
	

		m_PanelMap.Add("PanelAlert", "UI/PanelAlert");
		m_PanelMap.Add("PanelWait", "UI/PanelWait");


		m_PanelLayerMap.Add("PanelAlert", LayerPriority.Top);
		m_PanelLayerMap.Add("PanelWait", LayerPriority.Top);
	
	}

	public float GetUIScale() {
		Vector3 scale = m_Root.localScale;
		return scale.x;
	}

	public RectTransform ShowWindow(string window, object data=null) {
		if(!m_PanelMap.ContainsKey(window)) {
			Debug.Log(string.Format("Window {0} not found!", window));
			return null;
		}
		int index = m_Stack.FindIndex(x => x.name==window);
		UIView view = null;
		if (index >= 0) {
			view = m_Stack[index];
			m_Stack.RemoveAt(index);
		} else {
			Object obj = ResourceManager.GetInstance().LoadAsset(m_PanelMap[window]);
			GameObject panel = Instantiate(obj as GameObject);

			RectTransform rt = panel.GetComponent<RectTransform>();
			rt.SetParent(m_Root);
			rt.sizeDelta = new Vector2(0, 0);
			rt.localScale = Vector3.one;
			rt.localPosition = new Vector3(0, 0, 0);

			view = new UIView{name=window,rt=rt};
		}

		// set layer
		LayerPriority layer = LayerPriority.Normal;
		if (m_PanelLayerMap.ContainsKey(window)) layer = m_PanelLayerMap[window];
		if (m_Stack.Count > 0) {
			int idx = 0;
			for (; idx < m_Stack.Count; idx ++) {
				LayerPriority layer2 = LayerPriority.Normal;
				if (m_PanelLayerMap.ContainsKey(m_Stack[idx].name)) layer2 = m_PanelLayerMap[m_Stack[idx].name];
				if (layer2 > layer) {
					break;
				}
			}
			if (idx == m_Stack.Count) {
				m_Stack.Add(view);
				view.rt.SetAsLastSibling();
			} else {
				m_Stack.Insert(idx, view);
				//if (idx == 0)
					view.rt.SetSiblingIndex(m_Stack[idx].rt.GetSiblingIndex()-1);
				//else 
				//	view.rt.SetSiblingIndex(m_Stack[idx-1].rt.GetSiblingIndex());
			}
		} else {
			m_Stack.Add(view);
			view.rt.SetSiblingIndex(m_Root.childCount-1);
		}

		return view.rt;
	}


	public void HideWindow(string window) {
		int index = m_Stack.FindIndex(x => x.name==window);
		if (index >= 0) {
			Destroy(m_Stack[index].rt.gameObject);
			m_Stack.RemoveAt(index);
		}
	}
	public void HideWindow(RectTransform rt) {
		int index = m_Stack.FindIndex(x => x.rt==rt);
		if (index >= 0) {
			Destroy(m_Stack[index].rt.gameObject);
			m_Stack.RemoveAt(index);
		}
	}
	public void HideWindow(GameObject go) {
		RectTransform rt = go.GetComponent<RectTransform>();
		HideWindow(rt);
	}

	public bool IsWindowOpen(string window) {
		return m_Stack.Exists(x => x.name==window);
	}

	// 提示框
	public void ShowAlert(string title, string msg, AlertCallback callback=null) {
		RectTransform rt = ShowWindow("PanelAlert");

		PanelAlert script = rt.GetComponent<PanelAlert>();
		script.SetTitle(title);
		script.SetMsg(msg);
		script.SetCallback(callback);
	}
	public void HideAlert() {
		HideWindow("PanelAlert");
	}

	public Sprite LoadSprite(string spriteName) {
		return Resources.Load<GameObject>("UI/" + spriteName).GetComponent<SpriteRenderer>().sprite;
	}




}
