using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy {
	OrcController orcCtrl;

	Ability autoAttack;

	Renderer[] gosRends;

	void Init() {
		/*Thirang Level 1:
		 * Orc killed by:
		 	* 4 autoAttacks
		 	* 4 Berserk autoAttacks
		 	* 3 Cyclone Spin
		 	* 2 MagicArrow
		 	* 1 Magic Arrow and 1 AutoAttack
		 * Orc kills:
		 	* 60 autoAttacks
		 */ 
		this.health = 1000 * thirang.level;
		this.attackDamage = 50 * thirang.level;
		this.armor = 40 * thirang.level;
		this.magicResist = 25 * thirang.level;
		this.enemyType = EnemyType.Orc;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);
	}

	// Use this for initialization
	void Start () {
		orcCtrl = GetComponent<OrcController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		this.isAttacking = orcCtrl.isAttacking;

		if (!this.isAttacking)
			this.justAttacked = false;

		if (fadingDeath)
			FadeDeath (gosRends);

		ReadyNewAttack ();
	}

	void OnTriggerEnter (Collider other) {
		TriggerEnter (other);
	}
	

	public void OnAttack (Collider other) {
		DealDamageFighter (other, autoAttack);
	}

	public void OnDeath() {
		base.OnDeath (gold: 50, exp: 100);
	}
	
	protected override bool ThirangFacingEnemy () {
		return (thirang.FacingRight() && orcCtrl.isFacingLeft) || (!thirang.FacingRight() && !orcCtrl.isFacingLeft);
	}
}