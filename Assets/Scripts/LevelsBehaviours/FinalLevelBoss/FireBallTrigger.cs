using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallTrigger : MonoBehaviour {
	bool hit;

	void OnParticleCollision (GameObject other) {
		if (other.transform.root.CompareTag ("Player") && !hit) {
			hit = true;
			other.transform.root.SendMessage ("FireBallDamageController");
		}
	}
}
