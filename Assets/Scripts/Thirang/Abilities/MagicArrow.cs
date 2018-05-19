using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrow : MonoBehaviour {
	float arrowForce = 1000f;

	void Start () {
		GetComponent<Rigidbody> ().AddForce (Vector3.right * arrowForce);
		StartCoroutine (DestroyArrow ());
	}

	void Update() {
		transform.Rotate (new Vector3 (0, 0, 10f));
	}

	IEnumerator DestroyArrow() {
		yield return new WaitForSeconds (3f);
		Destroy(this.gameObject);
	}

	public void Direction(int way) {
		arrowForce = arrowForce * way;
	}
}
