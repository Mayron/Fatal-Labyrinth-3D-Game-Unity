using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public int loadingTime = 3;
	// Use this for initialization
	void Start () {
		StartCoroutine ("Loading");
	}
	
	IEnumerator Loading() {
		yield return new WaitForSeconds(loadingTime);
		GameObject music = GameObject.FindGameObjectWithTag ("MusicListener");
		if (music != null)
			GameObject.Destroy (music);
		Application.LoadLevel (Application.loadedLevel + 1);

	}
}
