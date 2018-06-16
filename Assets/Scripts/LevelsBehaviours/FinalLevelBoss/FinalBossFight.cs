using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossFight : MonoBehaviour {
	public GameObject movingGround;
	public GameObject thirang;
	public GameObject lavaMonster;
	public GameObject[] groundsBoss;
	public GameObject[] particleSystems;

	GroundRaycast[] rays;

	bool eventTriggered;
	bool startFight;

	void Start() {
		rays = thirang.GetComponentsInChildren<GroundRaycast> ();
	}

	void Update() {
		if (startFight) {
			if (rays [0].onGround && rays [1].onGround) {
				if (groundsBoss [0]) {
					if (thirang.transform.position.y < groundsBoss [0].transform.GetChild (0).position.y) {
						Destroy (groundsBoss [0]);
						Destroy (particleSystems [0]);
						print (groundsBoss.Length);
					}
				}

				if (groundsBoss [1]) {
					if (thirang.transform.position.y < groundsBoss [1].transform.GetChild (1).position.y) {
						Destroy (groundsBoss [1]);
						Destroy (particleSystems [1]);
						print (groundsBoss.Length);
					}
				}
			}
		}
	} 

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

			StartCoroutine (CameraAnimation (thirang));
		}
	}

	IEnumerator CameraAnimation (GameObject th) {
		yield return new WaitForSecondsRealtime (3.05f);
		Destroy (movingGround);

		th.GetComponent<ThirangController> ().enabled = true;
		th.GetComponent<AbilitiesController> ().enabled = true;

		yield return new WaitForSecondsRealtime (1f);
		lavaMonster.GetComponent<LavaMonsterController> ().enabled = true;
		startFight = true;

		Destroy (this.GetComponent<Collider> ());
	}

	IEnumerator ThirangToIdle(GameObject thirang) {
		yield return new WaitForSecondsRealtime (1f);
		thirang.GetComponent<Animator> ().CrossFade (ThirangSaT.idleStateHash, 0.4f);
	}
}
