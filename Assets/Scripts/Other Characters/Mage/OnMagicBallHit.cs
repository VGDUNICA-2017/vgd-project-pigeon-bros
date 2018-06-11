using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMagicBallHit : MonoBehaviour {
	public Mage mage { get; set; }
	float magicBallForce = 500f;
	bool hit;

	void Start () {
		GetComponent<Rigidbody> ().AddForce (Vector3.right * magicBallForce);
		StartCoroutine (DestroyMagicBall ());
	}

	IEnumerator DestroyMagicBall() {
		yield return new WaitForSeconds (8f);
		Destroy(this.gameObject);
	}

	public void Direction(int way) {
		magicBallForce = magicBallForce * way;

		if (way == -1) {
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 180, transform.eulerAngles.z);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !hit) {
			hit = true;
			mage.OnAttack (other);
		}
	}
}
