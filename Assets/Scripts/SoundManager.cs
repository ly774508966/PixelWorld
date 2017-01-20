using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

	// ui sound
	public static readonly string UI_BG_LOGIN = "UI/g_login";
	public static readonly string UI_BG_RAIN = "UI/g_rain";
	public static readonly string UI_BG_INWATER = "UI/p_inwater";

	public static readonly string UI_BTN = "UI/p_click";
	public static readonly string UI_GETITEM = "UI/p_getitem";

	// other
	public static readonly string ATTACK = "UI/p_attack";
	public static readonly string SHOOT = "UI/p_shot";
	public static readonly string PLACE = "UI/p_place";

	public static readonly string DOOR = "UI/p_door";

	private Dictionary<string, AudioSource> _dic_bgms = new Dictionary<string, AudioSource>();

	private static AudioSource _audioSourceSE;

	public static SoundManager instance;
	static SoundManager() {
		GameObject main = GameObject.Find("Main");
		if (main == null) {
			main = new GameObject("Main");
			DontDestroyOnLoad(main);
		}
		instance = main.AddComponent<SoundManager>();

	}

	void Awake() {
	}

	private AudioSource GetAudioSourceSE() {
		if (_audioSourceSE == null) {
			GameObject se = new GameObject("Audio SE");
			_audioSourceSE = se.AddComponent<AudioSource>();
		}
		return _audioSourceSE;
	}

	private AudioClip getAudioClip(string audioname) {
		return Resources.Load("music/"+audioname) as AudioClip;
	}

	public void PlayBGM(string name) {
		if (_dic_bgms.ContainsKey(name)) {
			AudioSource source = _dic_bgms[name];
			if (source.isPlaying) {
				Debug.LogWarning("audio already play!");
			} else {
				source.Play();
			}
		} else {
			AudioClip clip = getAudioClip(name);

			//Create the source	
			GameObject go = new GameObject("Audio: " + clip.name);
			AudioSource source = go.AddComponent<AudioSource>();	
			source.clip = clip;
			source.loop = true;
			source.Play ();

			_dic_bgms.Add(name, source);
		}
	}

	public void StopBGM(string name) {
		if (_dic_bgms.ContainsKey(name)) {
			AudioSource source = _dic_bgms[name];
			source.Stop();
		}
	}

	// sound effect
	public void PlaySE(string name) {
		AudioClip clip = getAudioClip(name);
		GetAudioSourceSE().PlayOneShot(clip);
	}

	public void Play(string name, Transform emitter) {
		AudioClip clip = getAudioClip(name);
		GameObject go = new GameObject ("Audio: " +  clip.name);
		go.transform.position = emitter.position;
		go.transform.parent = emitter;

		//Create the source	
		AudioSource source = go.AddComponent<AudioSource>();	
		source.clip = clip;
		source.spatialBlend = 1;
		source.rolloffMode = AudioRolloffMode.Custom;
		source.maxDistance = 30;
		source.Play ();
		Destroy (go, clip.length);
	}

	public void Play(string name, Vector3 position) {

		AudioClip clip = getAudioClip(name);
		GameObject go = new GameObject("Audio: " + clip.name);
		go.transform.position = position;

		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.spatialBlend = 1;
		source.Play();
		Destroy(go, clip.length);
	}


	public void Clear() {
		_dic_bgms.Clear();
	}
}
