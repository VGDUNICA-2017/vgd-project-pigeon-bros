using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeath : MonoBehaviour {
	public GameObject wallsOfDeath;
	public GameObject rightWall, leftWall;
	public GameObject movingGround1;
	public GameObject groundDisabled;
	Animator animWalls, animCamera, animRight, animLeft;
	LookAt camScript;
	MovingGround _movingGround1;

	bool triggeredEvent;

	void Start () {
		animWalls = wallsOfDeath.GetComponent<Animator> ();
		animRight = rightWall.GetComponent<Animator> ();
		animLeft = leftWall.GetComponent<Animator> ();
		animCamera = Camera.main.GetComponent<Animator> ();
		camScript = Camera.main.GetComponent<LookAt> ();
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !triggeredEvent) {
			triggeredEvent = true;
			camScript.enabled = false;
			animCamera.enabled = true;
			groundDisabled.SetActive (true);
			StartCoroutine (WallsOfDeath ());
			other.transform.root.GetComponent<JumpOverride> ().heightJump = 11f;
			other.transform.root.GetComponent<JumpOverride> ().runHeightJump = 11f;
		}
	}


	IEnumerator WallsOfDeath() {
		yield return new WaitForSeconds (Time.deltaTime);
		animCamera.SetTrigger ("Walls of Death");
		yield return new WaitForSecondsRealtime (1);
		animWalls.SetTrigger ("Walls of Death");
		StartCoroutine (GoingToDie ());
	}

	IEnumerator GoingToDie() {
		yield return new WaitForSecondsRealtime (1);
		animRight.SetTrigger ("Die");
		animLeft.SetTrigger ("Die");
		Destroy (this.gameObject);
	}
}
