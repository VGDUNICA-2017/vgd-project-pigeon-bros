using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour {
	static bool hit;

	void OnTriggerStay (Collider other) {
		if (other.transform.root.CompareTag ("Player") && transform.localPosition.y >= 0.58f && !hit) {
			hit = true;
			other.transform.root.GetComponent<Thirang> ().FatalDamage ();
		}
	}
}
