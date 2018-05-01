using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionArmAttack : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
		GetComponentInParent<Orc> ().OnAttack (other);
	}
}
