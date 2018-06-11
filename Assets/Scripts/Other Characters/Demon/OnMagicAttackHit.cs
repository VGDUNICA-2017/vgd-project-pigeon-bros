using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMagicAttackHit : MonoBehaviour {
	public Demon demon { get; set; }
	float magicAttackForce = 1000f;
	bool hit;

	void Start () {
		GetComponent<Rigidbody> ().AddForce (Vector3.right * magicAttackForce);
		StartCoroutine (DestroyMagicAttack ());
	}

	IEnumerator DestroyMagicAttack() {
		yield return new WaitForSeconds (3f);
		Destroy(this.gameObject);
	}

	public void Direction(int way) {
		magicAttackForce = magicAttackForce * way;


		//Change particle direction depending on spell cast direction
		if (way == 1) {
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 180, transform.eulerAngles.z);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !hit) {
			hit = true;
			demon.OnMagicCast (other);
		}
	}
}
