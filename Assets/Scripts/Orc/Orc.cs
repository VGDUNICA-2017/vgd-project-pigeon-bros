using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy {
	Animator anim;

	Ability autoAttack;

	int expAssigned;
	int goldAssigned;

	public Orc():base(1,1,1,1,1) {
		this.expAssigned = 100;
		this.goldAssigned = 50;
		this.enemyType = EnemyType.Orc;
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		autoAttack = new Ability (25, DamageType.physical);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
			if (stateInfo.fullPathHash == Animator.StringToHash ("Base Layer.Attack")) {
				OnDamage (other.gameObject, autoAttack);
			}
		}
	}
}
