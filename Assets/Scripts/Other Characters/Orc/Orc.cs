using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy {
	OrcController orcCtrl;

	Ability autoAttack;

	Renderer[] gosRends;

	int healthAtInit;

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
		this.health = 1000 + (this.health * ((thirang.level * 10) / 100));
		this.attackDamage = 50 + (this.attackDamage * ((thirang.level * 10) / 100));
		this.armor = 40 + (this.armor * ((thirang.level * 10) / 100));
		this.magicResist = 25 + (this.magicResist * ((thirang.level * 10) / 100));
		this.enemyType = EnemyType.Orc;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);

		healthAtInit = health;
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

		ReadyNewAttack_ThirangAlive ();
	}

	void OnTriggerEnter (Collider other) {
		TriggerEnter (other);
	}
	

	public void OnAttack (Collider other) {
		DealDamageFighter (other, autoAttack);
	}

	public void OnDeath() {
		base.OnDeath (gold: 50, exp: 100, health: healthAtInit / 10, mana: 10);
	}
	
	protected override bool ThirangEnemyFacingEachOther () {
		return (thirang.FacingRight() && orcCtrl.isFacingLeft) || (!thirang.FacingRight() && !orcCtrl.isFacingLeft);
	}
}