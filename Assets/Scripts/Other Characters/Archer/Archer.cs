using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy {
	ArcherController arcCtrl;

	Ability autoAttack;

	Renderer[] gosRends;

	void Init() {
		/*Thirang Level 1:
		 * Archer killed by:
		 	* 3 autoAttacks
		 	* 2 Berserk autoAttacks
		 	* 2 Cyclone Spin
		 	* 1 MagicArrow
		 	* 1 Cyclone Spin and 1 AutoAttack
		 * Orc kills:
		 	* 40 autoAttacks
		 */ 
		this.health = 800 * thirang.level;
		this.attackDamage = 75 * thirang.level;
		this.armor = 26 * thirang.level;
		this.magicResist = 25 * thirang.level;
		this.enemyType = EnemyType.Archer;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);
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

		ReadyNewAttack ();
	}

	void OnTriggerEnter (Collider other) {
		TriggerEnter (other);
	}
		
	public void OnAttack (Collider other) {
		DealDamageCaster (other, autoAttack);
	}

	public void OnDeath() {
		base.OnDeath (gold: 75, exp: 150);
	}

	protected override bool ThirangFacingEnemy() {
		return (thirang.FacingRight() && arcCtrl.isFacingLeft) || (!thirang.FacingRight() && !arcCtrl.isFacingLeft);
	}
}
