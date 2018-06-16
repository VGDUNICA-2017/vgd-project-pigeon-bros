using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGroundTrigger : MonoBehaviour {
	MovingGround mG;
	MovingGround.State state;
	bool controlState;

	void Update() {
		if (controlState) {
			if (mG.state != state) {
				mG.enabled = false;
				Destroy (this);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			mG = GetComponent<MovingGround> ();
			mG.enabled = true;
			StartCoroutine (WaitMovingGround ());
		}
	}

	IEnumerator WaitMovingGround() {
		yield return new WaitForSeconds (1f);
		controlState = true;
		state = mG.state;
	}
}
