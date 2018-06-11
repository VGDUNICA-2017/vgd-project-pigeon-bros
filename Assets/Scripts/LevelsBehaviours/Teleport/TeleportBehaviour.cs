using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportBehaviour : MonoBehaviour {
	Bounds teleportBounds;

	bool loaded;
	public string SceneToLoad;

	void Start() {
		teleportBounds = GetComponent <Collider> ().bounds;
	}
		
	void OnTriggerStay (Collider other) {
		if (other.transform.root.gameObject.CompareTag ("Player")) {
			Transform thirang = other.transform.root;
			if (!loaded && thirang.position.x > teleportBounds.min.x && thirang.position.x < teleportBounds.max.x) {

				if (!SceneToLoad.Contains ("Boss"))
					PlayerPrefs.SetInt ("gameLevel", PlayerPrefs.GetInt ("gameLevel") + 1);
				
				thirang.GetComponent<Thirang> ().SaveThirangData (false);
				StartCoroutine (LoadScene());
				loaded = true;
			}
		}
	}

	public void BossKilled(string scene) {
		SceneToLoad = scene;

		if (!SceneToLoad.Contains ("Boss"))
			PlayerPrefs.SetInt ("gameLevel", PlayerPrefs.GetInt ("gameLevel") + 1);

		GameObject thirang = GameObject.FindGameObjectWithTag ("Player");

		thirang.GetComponent<Thirang> ().SaveThirangData (false);
		StartCoroutine (LoadScene());
	}

	IEnumerator LoadScene() {
		AsyncOperation sceneLoad = SceneManager.LoadSceneAsync (SceneToLoad);

		while (!sceneLoad.isDone) {
			yield return null;

		}
	}
}