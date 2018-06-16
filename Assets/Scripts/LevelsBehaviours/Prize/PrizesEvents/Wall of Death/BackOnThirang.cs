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
			StartCoroutine (Wait ());
			other.transform.root.GetComponent<JumpOverride> ().heightJump = 9f;
			other.transform.root.GetComponent<JumpOverride> ().runHeightJump = 9f;
		}
	}

	IEnumerator Wait() {
		yield return new WaitForSecondsRealtime (3f);
		Destroy (animCamera);
		Destroy (this.gameObject);
	}
}
