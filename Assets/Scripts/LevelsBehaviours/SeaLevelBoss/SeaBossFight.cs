using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaBossFight : MonoBehaviour {
	public GameObject movingGround;
	public GameObject vibratingGround;
	VibratingGround[] vibratingCubes;

	bool eventTriggered;
	float speed;
	GameObject th;

	void Start() {
		vibratingCubes = vibratingGround.GetComponentsInChildren<VibratingGround> ();
		foreach (var v in vibratingCubes) {
			v.enabled = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !eventTriggered) {
			eventTriggered = true;

			th = other.transform.root.gameObject;

			th.GetComponent<ThirangController> ().enabled = false;
			th.GetComponent<Animator> ().SetFloat ("Speed", 0);
			StartCoroutine (ThirangToIdle ());

			Camera.main.GetComponent<LookAt> ().enabled = false;
			Camera.main.GetComponent<Animator> ().SetTrigger ("BossFight");

			StartCoroutine (StartFight ());
		}
	}

	IEnumerator StartFight() {
		yield return new WaitForSecondsRealtime (3.05f);

		th.GetComponent<ThirangController> ().enabled = true;
		Vector3 newPos = movingGround.transform.localPosition;
		newPos.x -= 40f;
		movingGround.transform.localPosition = newPos;
		foreach (var m in FindObjectsOfType<MovingGround> ()) {
			m.enabled = false;
		}

		ReactivateCubes ();

		Destroy (this.gameObject);
	}

	IEnumerator ThirangToIdle() {
		yield return new WaitForSecondsRealtime (1f);
		th.GetComponent<Animator> ().CrossFade (ThirangSaT.idleStateHash, 0.4f);
	}

	void ReactivateCubes() {
		foreach (var v in vibratingCubes) {
			v.enabled = true;
		}
	}
}
