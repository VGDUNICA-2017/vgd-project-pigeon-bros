using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBossFight : MonoBehaviour {
	public GameObject upperPart;
	public GameObject movingGroundBoss;
	public GameObject demon;

	bool eventTriggered;

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !eventTriggered) {
			eventTriggered = true;

			GameObject thirang = other.transform.root.gameObject;

			thirang.GetComponent<ThirangController> ().enabled = false;
			thirang.GetComponent<AbilitiesController> ().enabled = false;
			thirang.GetComponent<Animator> ().SetFloat ("Speed", 0);
			StartCoroutine (ThirangToIdle (thirang));

			Camera.main.GetComponent<LookAt> ().enabled = false;
			Camera.main.GetComponent<Animator> ().SetTrigger ("BossFight");

			StartCoroutine (UpperPart (thirang));
		}
	}

	IEnumerator UpperPart(GameObject th) {
		yield return new WaitForSecondsRealtime (3.05f);
		upperPart.GetComponent<Animator> ().SetTrigger ("GoDown");

		yield return new WaitForSecondsRealtime (3.05f);
		movingGroundBoss.GetComponent<MovingGround> ().enabled = true;

		th.GetComponent<ThirangController> ().enabled = true;
		th.GetComponent<AbilitiesController> ().enabled = true;

		demon.GetComponent<Animator> ().Play (DemonController.startStateHash);
		demon.GetComponent<DemonController> ().enabled = true;

		yield return new WaitForSecondsRealtime (1f);
		demon.tag = "Enemy";

		Destroy (this.gameObject);
	}

	IEnumerator ThirangToIdle(GameObject thirang) {
		yield return new WaitForSecondsRealtime (1f);
		thirang.GetComponent<Animator> ().CrossFade (ThirangSaT.idleStateHash, 0.4f);
	}
}
