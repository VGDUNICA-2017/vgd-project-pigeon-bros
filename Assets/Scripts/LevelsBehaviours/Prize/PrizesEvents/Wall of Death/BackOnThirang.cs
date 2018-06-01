using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackOnThirang : MonoBehaviour {
	Animator animCamera;
	LookAt camScript;

	bool triggeredEvent;

	// Use this for initialization
	void Start () {
		animCamera = Camera.main.GetComponent<Animator> ();
		camScript = Camera.main.GetComponent<LookAt> ();
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.transform.root.CompareTag ("Player") && !triggeredEvent) {
			triggeredEvent = true;
			camScript.enabled = true;
			animCamera.SetTrigger ("Back on Thirang");
		}
	}
}
