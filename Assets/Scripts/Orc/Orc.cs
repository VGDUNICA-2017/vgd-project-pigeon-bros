using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy {
	Animator anim;

	Ability autoAttack;

	int expAssigned;
	int goldAssigned;
	bool attacked = false;

	void Awake() {
		this.health = 1;
		this.attackDamage = 1;
		this.armor = 1;
		this.magicResist = 1;
		this.expAssigned = 100;
		this.goldAssigned = 50;
		this.enemyType = EnemyType.Orc;
		this.autoAttack = new Ability (25, DamageType.physical);
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		print (health);
	}

	void OnTriggerEnter (Collider other) {
		
	}

	public void OnAttack (Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
			if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
				if (!attacked) {	
					OnDamage (other.transform.root.gameObject, autoAttack);
					attacked = true;
				}
			} else {
				attacked = false;
			}
		}
	}
}