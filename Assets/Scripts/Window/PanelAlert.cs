using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum AlertRet {
	OK=0,
	CANCEL,
}
public delegate void AlertCallback(AlertRet ret);

public class PanelAlert : BaseWindow {
	private RectTransform btn_ok;
	private RectTransform btn_cancel;

	private Text text_title;
	private Text text_msg;

	public AlertCallback _callback;

	void Awake () {
		btn_ok = transform.FindChild("Button Ok") as RectTransform;
		btn_cancel = transform.FindChild("Button Cancel") as RectTransform;

		text_title = transform.FindChild("Text Title").GetComponent<Text>();
		text_msg = transform.FindChild("Text Msg").GetComponent<Text>();
	}

	public override void Init(object data) {
		transform.localPosition = new Vector3(0, 0, -1000);
	}

	public void SetTitle(string title) {
		text_title.text = title;
	}
	public void SetMsg(string msg) {
		text_msg.text = msg;
	}
	public void SetCallback(AlertCallback callback) {
		_callback = callback;
	}

	void Start() {
		if (_callback == null) {
			btn_ok.anchoredPosition = new Vector2(0, btn_ok.anchoredPosition.y);
			btn_cancel.gameObject.SetActive(false);
		}
	}

	public void OnBtnClose() {
		if (_callback != null) _callback(AlertRet.CANCEL);
		GUIManager.GetInstance().HideAlert();
		SoundManager.instance.PlaySE(SoundManager.UI_BTN);
	}

	public void OnBtnOk() {
		if (_callback != null) _callback(AlertRet.OK);
		GUIManager.GetInstance().HideAlert();
		SoundManager.instance.PlaySE(SoundManager.UI_BTN);
	}

	public void OnBtnCancel() {
		if (_callback != null) _callback(AlertRet.CANCEL);
		GUIManager.GetInstance().HideAlert();
		SoundManager.instance.PlaySE(SoundManager.UI_BTN);
	}
}
