using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotPrize : MonoBehaviour {
	public GameObject movingGround;
	public GameObject sign;

	bool eventTriggered;

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !eventTriggered) {
			eventTriggered = true;

			movingGround.SetActive (true);
			sign.SetActive (true);
			Destroy (this.gameObject);
		}
	}
}
