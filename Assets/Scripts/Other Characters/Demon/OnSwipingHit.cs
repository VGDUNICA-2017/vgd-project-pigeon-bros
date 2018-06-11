using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSwipingHit : MonoBehaviour {
	Demon demon;
	DemonController demonCtrl;

	void Start() {
		demonCtrl = GetComponentInParent<DemonController> ();
		demon = GetComponentInParent<Demon> ();
	}

	void OnTriggerEnter(Collider other) {
		if (demonCtrl.OnFrameToHit())
			demon.OnAttack (other);
	}
}
