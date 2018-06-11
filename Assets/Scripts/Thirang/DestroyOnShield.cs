using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnShield : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (GetComponentInParent<Animator> ().GetBool ("IsShielding")) {
			if (other.gameObject.CompareTag ("Destroyable"))
				Destroy (other.gameObject);
		}
	}
}
