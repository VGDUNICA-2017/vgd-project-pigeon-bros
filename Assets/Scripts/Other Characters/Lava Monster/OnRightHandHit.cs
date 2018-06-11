using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRightHandHit : MonoBehaviour {
	LavaMonster lavaMonster;
	LavaMonsterController lavaMonsterCtrl;

	void Start() {
		lavaMonsterCtrl = GetComponentInParent<LavaMonsterController> ();
		lavaMonster = GetComponentInParent<LavaMonster> ();
	}

	void OnTriggerEnter(Collider other) {
		if (lavaMonsterCtrl.OnFrameToHit())
			lavaMonster.OnAttack (other, "right");
	}
}
