using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonster : Enemy {
	LavaMonsterController lavaMonsterCtrl;

	Ability autoAttack;
	Ability doubleAutoAttack;

	Renderer[] gosRends;

	void Awake() {
		this.health = 40000;
		this.attackDamage = 320;
		this.armor = 100;
		this.magicResist = 120;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);
		this.doubleAutoAttack = new Ability (attackDamage * 2, DamageType.physical);
	}

	// Use this for initialization
	void Start () {
		lavaMonsterCtrl = GetComponent<LavaMonsterController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();
	}

	// Update is called once per frame
	void Update () {
		this.isAttacking = lavaMonsterCtrl.isAttacking;

		if (!this.isAttacking)
			this.justAttacked = false;

		if (fadingDeath)
			FadeDeath (gosRends);

		ReadyNewAttack ();

		print (health);
	}

	void OnTriggerEnter (Collider other) {
		if (!lavaMonsterCtrl.OnJumpAttack ())
			TriggerEnter (other);
		else
			TriggerEnter (other, false);
	}

	public void OnAttack (Collider other, string arm) {
		//Control if collider hit is thirang just for performance
		if (other.transform.root.CompareTag ("Player")) {
			if (!lavaMonsterCtrl.OnJumpAttackHitControl ()) {
				if (lavaMonsterCtrl.IsThisArmAttacking (arm))
					DealDamageFighter (other, autoAttack);
			} else {
				DealDamageFighter (other, doubleAutoAttack);
			}
		}
	}

	public void OnDeath() {
		base.OnDeath (gold: 5000, exp: 10000);
	}

	protected override bool ThirangFacingEnemy() {
		return (thirang.FacingRight() && lavaMonsterCtrl.isFacingLeft) || (!thirang.FacingRight() && !lavaMonsterCtrl.isFacingLeft);
	}

	public void OnMirrorAttack() {
		this.justAttacked = false;
	}
}

