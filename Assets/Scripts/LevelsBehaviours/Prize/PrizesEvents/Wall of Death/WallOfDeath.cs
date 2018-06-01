using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeath : MonoBehaviour {
	public GameObject wallsOfDeath;
	public GameObject rightWall, leftWall;
	public GameObject movingGround1;
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
		_movingGround1 = movingGround1.GetComponent<MovingGround> ();
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !triggeredEvent) {
			triggeredEvent = true;
			camScript.enabled = false;
			animCamera.SetTrigger ("Walls of Death");
			StartCoroutine (WallsOfDeath ());
		}
	}


	IEnumerator WallsOfDeath() {
		yield return new WaitForSecondsRealtime (1);
		animWalls.SetTrigger ("Walls of Death");
		StartCoroutine (GoingToDie ());
	}

	IEnumerator GoingToDie() {
		yield return new WaitForSecondsRealtime (1);
		animRight.SetTrigger ("Die");
		animLeft.SetTrigger ("Die");
		_movingGround1.enabled = true;
		Destroy (this.gameObject);
	}
}
