using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportBehaviour : MonoBehaviour {
	Bounds teleportBounds;
	public Transform thirangT;

	void Start() {
		teleportBounds = GetComponent <Collider> ().bounds;
	}
		
	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			if (thirangT.position.x > teleportBounds.min.x && thirangT.position.x < teleportBounds.max.x) {
				SceneManager.LoadScene ("LivelloMare");
			}
		}
	}
}
