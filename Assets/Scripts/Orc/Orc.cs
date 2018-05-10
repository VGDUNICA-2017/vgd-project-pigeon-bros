using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy {
	OrcController orcCtrl;

	Ability autoAttack;

	int expAssigned;
	int goldAssigned;
	bool attacked = false;

	public bool fadingDeath { get; set; }
	float deathTimer;

	void Awake() {
		this.health = 20;
		this.attackDamage = 1;
		this.armor = 10;
		this.magicResist = 1;
		this.expAssigned = 100;
		this.goldAssigned = 50;
		this.enemyType = EnemyType.Orc;
		this.autoAttack = new Ability (25, DamageType.physical);
	}

	// Use this for initialization
	void Start () {
		orcCtrl = GetComponent<OrcController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!orcCtrl.isAttacking)
			attacked = false;

		FadeDeathOrc ();
	}

	void OnTriggerEnter (Collider other) {
		
	}

	public void OnAttack (Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			if (orcCtrl.isAttacking) {
				if (!attacked) {	
					OnDamage (other.transform.root.gameObject, autoAttack);
					attacked = true;
				}
			} else {
				attacked = false;
			}
		}
	}

	public void OnDeath() {
		Thirang th = FindObjectOfType<Thirang> ();
		th.gold = goldAssigned;
		th.exp = expAssigned;
	}

	public void FadeDeathOrc() {
		if (fadingDeath) {
			deathTimer += Time.deltaTime;
			if (deathTimer >= 2f) {
				Renderer[] gosRends = GetComponentsInChildren<Renderer> ();
				foreach (Renderer r in gosRends) {
					print (r);
					Color newColor = r.material.color;
					print (r.material.color.a);
					newColor.a -= 20f;
					r.material.SetColor ("_Color", newColor);
				}
			}
		}
	}
}