using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLeftHandHit : MonoBehaviour {
	LavaMonster lavaMonster;
	LavaMonsterController lavaMonsterCtrl;

	void Start() {
		lavaMonsterCtrl = GetComponentInParent<LavaMonsterController> ();
		lavaMonster = GetComponentInParent<LavaMonster> ();
	}

	void OnTriggerEnter(Collider other) {
		if (lavaMonsterCtrl.OnFrameToHit())
			lavaMonster.OnAttack (other, "left");
	}
}
