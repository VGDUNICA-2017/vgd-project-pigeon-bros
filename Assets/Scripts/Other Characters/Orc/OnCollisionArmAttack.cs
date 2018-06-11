using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionArmAttack : MonoBehaviour {
	Orc orc;
	OrcController orcCtrl;

	void Start() {
		orcCtrl = GetComponentInParent<OrcController> ();
		orc = GetComponentInParent<Orc> ();
	}

	void OnTriggerEnter(Collider other) {
		if (orcCtrl.OnFrameToHit())
			orc.OnAttack (other);
	}
}
