using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyBindScript : MonoBehaviour {

	Dictionary<string, KeyCode> keys;
	public Text jump, forwards, backwards, 
	left, right, run, attack, block;

	GameObject currentKey;
	bool isShown = true;

	// Use this for initialization
	void Start () {
		keys = new Dictionary<string, KeyCode>();

		keys.Add ("Jump", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Jump", "Space"))); 
		jump.text = keys ["Jump"].ToString ();

		keys.Add ("Forwards", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Forwards", "W")));
		forwards.text = keys ["Forwards"].ToString ();

		keys.Add ("Backwards", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Backwards", "S")));
		backwards.text = keys ["Backwards"].ToString ();

		keys.Add ("Left", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Left", "A")));
		left.text = keys ["Left"].ToString ();

		keys.Add ("Right", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Right", "D")));
		right.text = keys ["Right"].ToString ();

		keys.Add ("Run", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Run", "LeftShift")));
		run.text = keys ["Run"].ToString ();

		keys.Add ("Attack", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Attack", "Mouse0")));
		attack.text = keys ["Attack"].ToString ();

		keys.Add ("Block", (KeyCode)System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Block", "Mouse1")));
		block.text = keys ["Block"].ToString ();

		Toggle ();
	}
	
	public void Toggle() {
		isShown = !isShown;
		foreach (var key in keys) {
			PlayerPrefs.SetString (key.Key, key.Value.ToString ());
		}
		PlayerPrefs.Save ();

		gameObject.SetActive (isShown);
	}

	void OnGUI() {
		if (currentKey != null) {
			Event e = Event.current;
			if (e.isKey) {
				keys[currentKey.name] = e.keyCode;
				currentKey.GetComponent<Text>().text = e.keyCode.ToString ();
				currentKey = null;
			}
		}
	}

	public void ChangeKey(GameObject clicked) {
		currentKey = clicked;

	}
}
