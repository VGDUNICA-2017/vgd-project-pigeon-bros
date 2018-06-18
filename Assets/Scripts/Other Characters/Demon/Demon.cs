using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy {
	DemonController demonCtrl;
	public ParticleSystem spawn;

	Ability autoAttack;
	Ability magicAttack;

	Renderer[] gosRends;

	ThirangController thCtrl;
	bool thOnLowerGround, thOnUpperGround; 

	void Awake() {
		/*Thirang Level 1:
		 * Demon killed by:
		 	* 50 autoAttacks
		 	* 46 Berserk autoAttacks
		 	* 36 Cyclone Spin
		 	* 25 MagicArrow
		 * Demon kills:
		 	* 15 autoAttacks
		 	* 10 autoAttacks and 2 magicAttacks
		 	* 6 magicAttacks
		 */ 
		this.health = 10000;
		this.attackDamage = 200;
		this.armor = 50;
		this.magicResist = 75;
		this.autoAttack = new Ability (attackDamage, DamageType.physical);
		this.magicAttack = new Ability (attackDamage * 240 / 100, DamageType.magic);
	}

	// Use this for initialization
	void Start () {
		demonCtrl = GetComponent<DemonController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();
		thCtrl = thirang.gameObject.GetComponent<ThirangController> ();

		thOnLowerGround = true;
	}

	// Update is called once per frame
	void Update () {
		this.isAttacking = demonCtrl.isAttacking;

		if (!this.isAttacking)
			this.justAttacked = false;

		if (fadingDeath)
			FadeDeath (gosRends);

		ReadyNewAttack_ThirangAlive ();

		if (demonCtrl.enabled) {
			//Executed on first time thirang lies on selected ground
			if (thCtrl.onDemonUpperGround) {
				if (!thOnUpperGround) {
					thOnUpperGround = true;
					StartCoroutine (TeleportDemon (1));
				}
			} else {
				thOnUpperGround = false;
			}

			if (thCtrl.onDemonLowerGround) {
				if (!thOnLowerGround) {
					thOnLowerGround = true;
					StartCoroutine (TeleportDemon (-1));
				}
			} else {
				thOnLowerGround = false;
			}
		}
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
		base.OnDeath (gold: 500, exp: 1200, health: thirang.maxHealth, mana: thirang.maxMana);
	}

	protected override bool ThirangEnemyFacingEachOther() {
		return (thirang.FacingRight() && demonCtrl.isFacingLeft) || (!thirang.FacingRight() && !demonCtrl.isFacingLeft);
	}

	private IEnumerator TeleportDemon (int plane) {
		Animator anim = GetComponent<Animator> ();
		demonCtrl.enabled = false;

		yield return new WaitForSecondsRealtime (1.5f);

		anim.applyRootMotion = false;
		transform.position = new Vector3 (transform.position.x, transform.position.y + 11 * plane);
		ParticleSystem _ps = Instantiate (spawn, transform.position, Quaternion.identity, null);
		anim.applyRootMotion = true;
		demonCtrl.enabled = true;
		yield return new WaitForSecondsRealtime (2f);
		Destroy (_ps.gameObject);
	}
}
