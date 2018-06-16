using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonster : Enemy {
	LavaMonsterController lavaMonsterCtrl;

	Ability autoAttack;

	//Double autoAttack is an ability half physical damage, half magic damage
	Ability doubleAutoAttack_physical;
	Ability doubleAutoAttack_magic;

	Renderer[] gosRends;

	void Awake() {
		/*Thirang Level 1:
		 * Lava Monster killed by:
		 	* 123 autoAttacks
		 	* 112 Berserk autoAttacks
		 	* 88 Cyclone Spin
		 	* 57 MagicArrow
		 * Lava Monster kills:
		 	* 12 autoAttacks
		 	* 6 autoAttacks and 3 jumpAttack
		 	* 6 jumpAttack
		 */
		this.health = 17500;
		this.attackDamage = 250;
		this.armor = 70;
		this.magicResist = 65;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);
		this.doubleAutoAttack_physical = new Ability (attackDamage, DamageType.physical);
		this.doubleAutoAttack_magic = new Ability (attackDamage, DamageType.magic);
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

		ReadyNewAttack_ThirangAlive ();

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
				DealDamageFighter (other, new Ability[] { doubleAutoAttack_physical, doubleAutoAttack_magic } );
			}
		}
	}

	public void OnDeath() {
		base.OnDeath (gold: 5000, exp: 10000, health: thirang.maxHealth, mana: thirang.maxMana);
	}

	protected override bool ThirangEnemyFacingEachOther() {
		return (thirang.FacingRight() && lavaMonsterCtrl.isFacingLeft) || (!thirang.FacingRight() && !lavaMonsterCtrl.isFacingLeft);
	}

	public void OnMirrorAttack() {
		this.justAttacked = false;
	}
}

