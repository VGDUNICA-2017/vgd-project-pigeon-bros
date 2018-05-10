using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportBehaviour : MonoBehaviour {
	Bounds teleportBounds;

	bool loaded;

	void Start() {
		teleportBounds = GetComponent <Collider> ().bounds;
	}
		
	void OnTriggerStay (Collider other) {
		if (other.transform.root.gameObject.CompareTag ("Player")) {
			Transform thirang = other.transform.root;
			if (thirang.position.x > teleportBounds.min.x && thirang.position.x < teleportBounds.max.x && !loaded) {
				StartCoroutine (LoadScene());
				loaded = true;
			}
		}
	}

	IEnumerator LoadScene() {
		AsyncOperation sceneLoad = SceneManager.LoadSceneAsync ("LivelloMare");

		while (!sceneLoad.isDone) {
			yield return null;

		}
	}
}