using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotPrize : MonoBehaviour {
	public GameObject movingGround1;
	public GameObject movingGround2;
	public GameObject sign;
	public GameObject deathZone;
	public GameObject movingGroundToDestroy;

	bool eventTriggered;

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !eventTriggered) {
			eventTriggered = true;

			movingGround1.SetActive (true);
			movingGround2.SetActive (true);
			sign.SetActive (true);
			deathZone.SetActive (true);
			Destroy (movingGroundToDestroy);
		}
	}
}
