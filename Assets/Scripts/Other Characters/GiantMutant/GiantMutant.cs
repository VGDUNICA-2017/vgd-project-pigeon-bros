using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantMutant : Enemy {
	GiantMutantController giantMutantCtrl;

	Ability autoAttack;

	Renderer[] gosRends;

	void Awake() {
		this.health = 25000;
		this.attackDamage = 160;
		this.armor = 100;
		this.magicResist = 70;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);
	}

	// Use this for initialization
	void Start () {
		giantMutantCtrl = GetComponent<GiantMutantController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();
	}

	// Update is called once per frame
	void Update () {
		this.isAttacking = giantMutantCtrl.isAttacking;

		if (!this.isAttacking)
			this.justAttacked = false;

		if (fadingDeath)
			FadeDeath (gosRends);

		ReadyNewAttack ();
	}

	void OnTriggerEnter (Collider other) {
		TriggerEnter (other);
	}

	public void OnAttack (Collider other, string arm) {
		//Control if collider hit is thirang just for performance
		if (other.transform.root.CompareTag ("Player")) {
			if (giantMutantCtrl.IsThisArmAttacking (arm))
				DealDamageFighter (other, autoAttack);
		}
	}

	public void OnDeath() {
		base.OnDeath (gold: 750, exp: 1500);
	}

	protected override bool ThirangFacingEnemy() {
		return (thirang.FacingRight() && giantMutantCtrl.isFacingLeft) || (!thirang.FacingRight() && !giantMutantCtrl.isFacingLeft);
	}

	public void OnMirrorAttack() {
		this.justAttacked = false;
	}
}
