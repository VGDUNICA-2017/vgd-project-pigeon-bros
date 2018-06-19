using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
	bool triggered;

	void OnTriggerEnter(Collider other) {
		if (!triggered && other.transform.root.CompareTag ("Player")) {
			triggered = true;
			other.transform.root.GetComponent<Thirang> ().FatalDamage ();
			if (Camera.main.GetComponent<LookAt> ())
				Camera.main.GetComponent<LookAt> ().enabled = false;
		}
	}
}
