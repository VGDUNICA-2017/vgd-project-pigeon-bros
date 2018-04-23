using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSword : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		GetComponentInParent<Thirang> ().OnCollisionSword (other);
	}
}
