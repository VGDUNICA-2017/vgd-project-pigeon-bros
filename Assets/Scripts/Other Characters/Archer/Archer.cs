using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy {
	ArcherController arcCtrl;

	Ability autoAttack;

	Renderer[] gosRends;

	int healthAtInit;

	void Init() {
		/*Thirang Level 1:
		 * Archer killed by:
		 	* 3 autoAttacks
		 	* 2 Berserk autoAttacks
		 	* 2 Cyclone Spin
		 	* 1 MagicArrow
		 	* 1 Cyclone Spin and 1 AutoAttack
		 * Thirang kills:
		 	* 40 autoAttacks
		 */ 
		this.health = 800 + (this.health * ((thirang.level * 10) / 100));
		this.attackDamage = 75 + (this.attackDamage * ((thirang.level * 10) / 100));
		this.armor = 26 + (this.armor * ((thirang.level * 10) / 100));
		this.magicResist = 25 + (this.magicResist * ((thirang.level * 10) / 100));
		this.enemyType = EnemyType.Archer;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);

		healthAtInit = health;
	}

	// Use this for initialization
	void Start () {
		arcCtrl = GetComponent<ArcherController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();

		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (fadingDeath)
			FadeDeath (gosRends);

		ReadyNewAttack_ThirangAlive ();
	}

	void OnTriggerEnter (Collider other) {
		TriggerEnter (other);
	}
		
	public void OnAttack (Collider other) {
		DealDamageCaster (other, autoAttack);
	}

	public void OnDeath() {
		base.OnDeath (gold: 75, exp: 150, health: healthAtInit / 10, mana: 5);
	}

	protected override bool ThirangEnemyFacingEachOther() {
		return (thirang.FacingRight() && arcCtrl.isFacingLeft) || (!thirang.FacingRight() && !arcCtrl.isFacingLeft);
	}
}
