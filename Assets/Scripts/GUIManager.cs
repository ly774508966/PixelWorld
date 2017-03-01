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
	public RectTransform Root {
		get {
			if (m_Root == null) {
				Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
				m_Root = canvas.GetComponent<RectTransform>();
			}
			return m_Root;
		}
	}

	private Dictionary<string, LayerPriority> m_PanelLayerMap = new Dictionary<string, LayerPriority>();
	private List<UIView>		m_Stack = new List<UIView>();


	void Awake () {

		m_PanelLayerMap.Add("PanelAlert", LayerPriority.Top);
		m_PanelLayerMap.Add("PanelWait", LayerPriority.Top);
	
	}

	public float GetUIScale() {
		Vector3 scale = Root.localScale;
		return scale.x;
	}

	public RectTransform ShowWindow(string window, object data=null) {
		int index = m_Stack.FindIndex(x => x.name==window);
		UIView view = null;
		if (index >= 0) {
			view = m_Stack[index];
			m_Stack.RemoveAt(index);
		} else {
			Object obj = ResourceManager.GetInstance().LoadAsset("UI/"+window);
			GameObject panel = Instantiate(obj as GameObject);
			panel.name = window;
			LuaBehaviour lua = panel.GetComponent<LuaBehaviour>();
			if (lua == null) panel.AddComponent<LuaBehaviour>();

			RectTransform rt = panel.GetComponent<RectTransform>();
			rt.SetParent(Root);
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
				view.rt.SetAsLastSibling();
				m_Stack.Add(view);
			} else {
				int siblingIndex = m_Stack[idx].rt.GetSiblingIndex();
				for (int i = m_Stack.Count -1; i >= idx; i --) {
					m_Stack[idx].rt.SetSiblingIndex(m_Stack[idx].rt.GetSiblingIndex() + 1);
				}
				view.rt.SetSiblingIndex(siblingIndex);
				//if (idx == 0)
					//view.rt.SetSiblingIndex(m_Stack[idx].rt.GetSiblingIndex()-1);
				//else 
				//	view.rt.SetSiblingIndex(m_Stack[idx-1].rt.GetSiblingIndex());

				m_Stack.Insert(idx, view);
			}
		} else {
			view.rt.SetSiblingIndex(Root.childCount-1);
			m_Stack.Add(view);
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

	public void Clear() {
		m_Stack.Clear();
	}
}
