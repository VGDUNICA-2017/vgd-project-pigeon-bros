using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaBossFight : MonoBehaviour {
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
		print ("before wait");
		yield return new WaitForSecondsRealtime (3.05f);
		print ("after wait");

		th.GetComponent<ThirangController> ().enabled = true;
		print("thirangControllerEnabled");

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
