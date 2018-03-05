using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionThirang : MonoBehaviour {

	void OnCollisionEnter (Collision other) {
		GetComponentInParent<Thirang> ().OnCollisionEvent (other);
	}
}
