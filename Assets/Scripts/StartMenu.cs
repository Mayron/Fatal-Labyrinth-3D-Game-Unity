using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	GameObject musicListener;

	void Awake() {
		musicListener = GameObject.FindGameObjectWithTag ("MusicListener");
	}

	public void Continue_OnClick() {
		// load settings first
		GameObject.DontDestroyOnLoad (musicListener);
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void NewGame_OnClick() {
		GameObject.DontDestroyOnLoad (musicListener);
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void Quit_OnClick() {
		Debug.Log ("Quitting Game..");
		Application.Quit ();
	}
}
