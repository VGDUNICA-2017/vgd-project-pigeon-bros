using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRightAttackHit : MonoBehaviour {
	GiantMutant giantMutant;
	GiantMutantController giantMutantCtrl;

	void Start() {
		giantMutantCtrl = GetComponentInParent<GiantMutantController> ();
		giantMutant = GetComponentInParent<GiantMutant> ();
	}

	void OnTriggerEnter(Collider other) {
		if (giantMutantCtrl.OnFrameToHit())
			giantMutant.OnAttack (other, "right");
	}
}
