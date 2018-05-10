using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeGroundBoss : MonoBehaviour {
	bool toFade = false;
	bool toUn_Fade = false;
	bool startCount = false;
	float timer = 0f;
	Color baseColor, color, newColor;

	void Start() {
		baseColor = gameObject.GetComponent<Renderer> ().material.color;
		newColor = baseColor;
	}

	// Update is called once per frame
	void Update () {
		if (toFade) {
			Fade ();
		}
		if (toUn_Fade) {
			Un_Fade ();
		}

		if (startCount) {
			timer += Time.deltaTime;
			if (timer >= 4f) {
				timer = 0f;
				startCount = false;
				toUn_Fade = true;
			}
		}
	}

	void Fade() {
		toUn_Fade = false;
		newColor.a -= 2f;
		gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", newColor);
		if (gameObject.GetComponent<Renderer>().material.color.a == 0f) {
			startCount = true;
			toFade = false;
		}
	}

	void Un_Fade() {
		newColor.a += 2f;
		gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", newColor);
		if (newColor.a >= baseColor.a) {
			gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", baseColor);
			toUn_Fade = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Boss")) {
			toFade = true;
		}
	}
}
