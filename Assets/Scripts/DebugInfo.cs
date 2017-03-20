using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugInfo : MonoBehaviour {

	public GUIStyle guiStyle;
	public GUIStyle guiStyleError;
	
	public class Log {
		public string msg;
		public string stacktrace;
		public LogType type;
		public Log(string msg, string stacktrace, LogType type) {
			this.msg = msg;
			this.stacktrace = stacktrace;
			this.type = type;
		}
	}
	static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>  
        {  
            { LogType.Assert, Color.white },  
            { LogType.Error, Color.red },  
            { LogType.Exception, Color.red },  
            { LogType.Log, Color.white },  
            { LogType.Warning, Color.yellow },  
        };  

	private double 		lastInterval = 0.0;
	private int 		frames = 0;
	private float 		m_fps;
	private float 		m_accumTime = 0;
	private float 		m_fpsUpdateInterval = 0.5f;

	private string 		strFPS;
	private string 		strMem;

	private Log 		log;
	private string 		strError;

	void Awake() {
		DontDestroyOnLoad(gameObject);

		Texture2D tex = new Texture2D(128, 128);
		for (int y = 0; y < tex.height; y ++) {
			for (int x = 0; x < tex.width; x ++) {
				tex.SetPixel(x,y, Color.gray);
			}
		}
		guiStyle.normal.background = tex;
	}

	void Start () {
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}

	void HandleLog(string msg, string stacktrace, LogType type) {
		if (type == LogType.Assert || type == LogType.Error || type == LogType.Exception) {
			log = new Log(msg, stacktrace, type);
		}
	}

	void OnEnable() {
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable () {
		Application.logMessageReceived -= HandleLog;
	}

	// Update is called once per frame
	void Update () {
		++frames;
		//*
		float timeNow = Time.realtimeSinceStartup;
		if (timeNow - lastInterval > m_fpsUpdateInterval) {
			float fps =  frames /(float) (timeNow - lastInterval);
			float ms = 1000.0f / Mathf.Max(fps, 0.0001f);
			strFPS = string.Format("{0} ms {1}FPS", ms.ToString("f1"), fps.ToString("f2"));
			frames = 0;
			lastInterval = timeNow;
		}
		/*/
		/*
		float dt = Time.deltaTime/Time.timeScale;
		m_accumTime += dt;
		if (m_accumTime >= m_fpsUpdateInterval) {
			m_fps = frames/m_accumTime;
			m_accumTime = 0.0f;
			frames = 0;
			float ms = 1000.0f / Mathf.Max(m_fps, 0.0001f);
			strFPS = string.Format("{0} ms {1}FPS", ms.ToString("f1"), m_fps.ToString("f2"));
		}
		*/
		// system info
		if (Time.frameCount % 30 == 0) {
			strMem = string.Format("memory used : {0} MB", System.GC.GetTotalMemory(false) / (1024*1024));
		}

		// log
		if (log != null) {
			strError = log.msg + "\n" + log.stacktrace;
			log = null;
		}
	}


	void OnGUI()  {  	
		GUI.backgroundColor = Color.gray;

		GUI.Box(new Rect(5, 10, 200, 20), strFPS , guiStyle);  
		GUI.Box(new Rect(5, 30, 200, 20),  strMem, guiStyle); 
		GUI.Box(new Rect(5, 50, 400, 20),  strError, guiStyleError); 
	} 
}
