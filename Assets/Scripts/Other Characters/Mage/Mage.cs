using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Enemy {
	MageController mageCtrl;

	Ability magicAttack;

	Renderer[] gosRends;

	public bool bossFightHit { get; set; }

	void Awake() {
		//Thirang Level 1
		//20 hits to kill Thirang, 6 Thirang's autoAttacks to be killed
		this.health = 6000;
		this.attackDamage = 144;
		this.armor = 10;
		this.magicResist = 70;
		this.magicAttack = new Ability (attackDamage, DamageType.magic);
	}

	// Use this for initialization
	void Start () {
		mageCtrl = GetComponent<MageController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();
	}

	// Update is called once per frame
	void Update () {
		if (fadingDeath)
			FadeDeath (gosRends);

		ReadyNewAttack_ThirangAlive ();

		print (health);
	}

	void OnTriggerEnter (Collider other) {
		if (!bossFightHit && other.CompareTag ("Sword") && thirang.Fighting()) {
			TriggerEnter (other);
			bossFightHit = true;
		}
	}

	public void OnAttack (Collider other) {
		DealDamageCaster (other, magicAttack);
	}

	public void OnDeath() {
		base.OnDeath (gold: 200, exp: 400, health: thirang.maxHealth, mana: thirang.maxMana);
	}

	protected override bool ThirangEnemyFacingEachOther() {
		return (thirang.FacingRight() && mageCtrl.isFacingLeft) || (!thirang.FacingRight() && !mageCtrl.isFacingLeft);
	}
}
