using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSword : MonoBehaviour {

	void OnCollisionEnter (Collision other) {
		GetComponentInParent<Thirang> ().OnCollisionSword (other);
	}
}
