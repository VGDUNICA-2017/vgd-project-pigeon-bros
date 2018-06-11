using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy {
	DemonController demonCtrl;

	Ability autoAttack;
	Ability magicAttack;

	Renderer[] gosRends;

	void Awake() {
		this.health = 18000;
		this.attackDamage = 130;
		this.armor = 75;
		this.magicResist = 75;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);
		this.magicAttack = new Ability (attackDamage * 150 / 100, DamageType.magic);
	}

	// Use this for initialization
	void Start () {
		demonCtrl = GetComponent<DemonController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();
	}

	// Update is called once per frame
	void Update () {
		this.isAttacking = demonCtrl.isAttacking;

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

	public void OnMagicCast (Collider other) {
		DealDamageCaster (other, magicAttack);
	}

	public void OnDeath() {
		base.OnDeath (gold: 500, exp: 1000);
	}

	protected override bool ThirangFacingEnemy() {
		return (thirang.FacingRight() && demonCtrl.isFacingLeft) || (!thirang.FacingRight() && !demonCtrl.isFacingLeft);
	}
}
