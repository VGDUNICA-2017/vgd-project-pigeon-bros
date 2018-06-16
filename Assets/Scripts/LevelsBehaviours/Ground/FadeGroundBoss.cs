using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeGroundBoss : MonoBehaviour {
	bool toFade = false;
	bool toUn_Fade = false;
	bool startCount = false;
	bool notUn_Fade = false;
	float timer = 0f;
	Renderer[] rendCubes;
	Collider colliderCubes;
	Color baseColor, color, newColor;

	void Start() {
		rendCubes = GetComponentsInChildren<Renderer> ();
		colliderCubes = GetComponent<Collider> ();
		baseColor = rendCubes[0].material.color;
		newColor = baseColor;
	}

	// Update is called once per frame
	void Update () {
		if (startCount && !notUn_Fade) {
			timer += Time.deltaTime;
			if (timer >= 2.5f) {
				timer = 0f;
				startCount = false;
				toUn_Fade = true;
			}
		} else {
			timer = 0f;
		}
	}

	void LateUpdate() {
		if (toFade) {
			Fade ();
		}
		if (toUn_Fade) {
			Un_Fade ();
		}
	}

	void Fade() {
		toUn_Fade = false;
		newColor.a -= 0.01f;
		foreach (var r in rendCubes) {
			r.material.SetColor ("_Color", newColor);
		}

		if (newColor.a <= baseColor.a / 2) {
			colliderCubes.isTrigger = true;
		}

		if (rendCubes[0].material.color.a <= 0f) {
			newColor.a = 0f;
			foreach (var r in rendCubes) {
				r.material.SetColor ("_Color", newColor);
			}
			startCount = true;
			toFade = false;
		}
	}

	void Un_Fade() {
		newColor.a += 0.01f;
		foreach (var r in rendCubes) {
			r.material.SetColor ("_Color", newColor);
		}

		if (newColor.a >= baseColor.a / 2) {
			colliderCubes.isTrigger = false;
		}

		if (newColor.a >= baseColor.a) {
			foreach (var r in rendCubes) {
				r.material.SetColor ("_Color", baseColor);
			}
			toUn_Fade = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.CompareTag ("Enemy")) {
			print ("collisionEnemy");
			toFade = true;
			toUn_Fade = false;
			newColor = baseColor;
		}

		if (other.transform.CompareTag ("ThirangFadeTrigger")) {
			notUn_Fade = true;
			toUn_Fade = false;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.transform.CompareTag ("Enemy")) {
			notUn_Fade = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.transform.CompareTag ("Enemy")) {
			notUn_Fade = false;
		}

		if (other.transform.CompareTag ("ThirangFadeTrigger")) {
			notUn_Fade = false;
			if (newColor.a < baseColor.a)
				toUn_Fade = true;
		}
	}
}
