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
				loaded = true;
				thirang.GetComponent<Thirang> ().SaveThirangData (false);
				PlayerPrefs.SetString ("Scene", UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name + "Boss");
				PlayerPrefs.Save ();
				StartCoroutine (LoadScene());
			}
		}
	}

	public void BossKilled(string scene) {
		SceneToLoad = scene;

		PlayerPrefs.SetInt ("gameLevel", PlayerPrefs.GetInt ("gameLevel") + 1);

		GameObject thirang = GameObject.FindGameObjectWithTag ("Player");

		thirang.GetComponent<Thirang> ().SaveThirangData (false);
		PlayerPrefs.SetString ("Scene", SceneToLoad);
		PlayerPrefs.Save ();
		StartCoroutine (LoadScene());
	}

	public void FinalBoss(string scene) {
		SceneToLoad = scene;

		StartCoroutine (LoadScene ());
	}

	IEnumerator LoadScene() {
		AsyncOperation sceneLoad = SceneManager.LoadSceneAsync (SceneToLoad);

		while (!sceneLoad.isDone) {
			yield return null;

		}
	}
}