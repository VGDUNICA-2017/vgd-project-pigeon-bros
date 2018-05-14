using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrow : MonoBehaviour {
	float arrowForce = 1000f;
	GameObject thirang;
	Thirang th;

	void Start () {
		GetComponent<Rigidbody> ().AddForce (Vector3.right * arrowForce);
	}

	public void SetArrowCaster(GameObject g) {
		thirang = g;
		th = thirang.GetComponent<Thirang> ();
	}
}
