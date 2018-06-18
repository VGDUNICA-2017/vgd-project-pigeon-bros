using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestruction : MonoBehaviour {
	public GameObject rockDestroyed;
	public float yOffset = 8f;

	Thirang thirang;

	bool eventTriggered1, eventTriggered2;

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !eventTriggered1) {
			eventTriggered1 = true;
			thirang = other.transform.root.GetComponent<Thirang> ();
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			if (thirang.SpecialAbility (this.tag) && !eventTriggered2) {
				if (Input.GetButtonDown ("specialAbility")) {
					eventTriggered2 = true;
					DestroyRock ();
				}
			}
		}
	}

	public void DestroyRock() {
		Vector3 pos = transform.parent.position;
		pos.y += yOffset;
		Instantiate (rockDestroyed, pos, Quaternion.identity);
		Destroy (transform.parent.gameObject);
	}
}
