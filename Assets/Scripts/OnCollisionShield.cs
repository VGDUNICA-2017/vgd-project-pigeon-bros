using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionShield : MonoBehaviour {

	void OnCollisionEnter (Collision other) {
		GetComponentInParent<Thirang> ().OnCollisionShield (other);
	}
}
