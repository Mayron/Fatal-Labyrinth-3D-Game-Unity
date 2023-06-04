using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public GameObject menu;
	public KeyBindScript kb;
	public AudioSource ambience;

	bool isShown = true;

	void Start() {
		Toggle ();
	}
	
	public void Ambience_OnValueChanged(float value) {
		//Debug.Log (value);
		if (Application.loadedLevelName == "Level01" || 
		    Application.loadedLevelName == "Level02") {
			ambience.volume = value;
		}
	}

	public void Combat_OnValueChanged(float value) {
		//Debug.Log (value);
	}

	public void Difficulty_OnValueChanged(int value) {
		Statistics.ChangeDifficulty (value);
	}

	public void Apply() {
		Toggle ();
	}

	public void Toggle() {
		isShown = !isShown;
		menu.SetActive (isShown);
		if (isShown && Application.loadedLevelName == "Level01") {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	public void SaveGame() {

	}

	public void ShowKeyBindings() {
		kb.Toggle ();
	}

	public bool IsShown() {
		return isShown;
	}

	public void ExitToStartMenu() {
		Application.LoadLevel (0);
	}

}
