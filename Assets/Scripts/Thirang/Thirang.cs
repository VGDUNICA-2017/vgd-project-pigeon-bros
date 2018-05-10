using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirang : Character {
	Animator anim;

	int mana { get; set; }
	int level;
	public int gold { get; set; }
	public int exp { get; set; }
	const int TO_LEVEL_2 = 10000;
	const int TO_LEVEL_3 = 80000;
	public enum ThirangConstructor { _default , _save }; 

	Ability autoAttack;
	Ability berserk;
	Ability cycloneSpin;
	Ability magicArrow;
	private Ability currAbility;

	public string currentAbility 
	{
		set {
			switch (value) 
			{
				case "autoAttack": 
					currAbility = autoAttack;
					break;
				case "berserk":
					currAbility = berserk;
					break;
				case "cycloneSpin":
					currAbility = cycloneSpin;
					break;
				case "magicArrow":
					currAbility = magicArrow;
					break;
				default:
					throw new System.ArgumentException ("Cannot find ability with this name");
			}
		}
	}

	int berserkTime;
	float timer;
	bool berserkIsActive;
	public bool newAttack { get; set; }

	public void Init(ThirangConstructor init) {
		switch (init) {
			case ThirangConstructor._default:
				health = 1200;
				mana = 300;
				attackDamage = 50;
				armor = 20;
				magicResist = 15;
				level = 1;
				autoAttack = new Ability (attackDamage, DamageType.physical);
				berserk = new Ability (attackDamage * 110 / 100, DamageType.physical);
				cycloneSpin = new Ability (attackDamage * 120 / 100, DamageType.magic);
				magicArrow = new Ability (attackDamage * 200 / 100, DamageType.magic);
				berserkTime = 3 * level;
				break;
			case ThirangConstructor._save:
				break;
			default:
				throw new System.ArgumentException ("Error initializing Thirang");
		}
	}

	void Awake() {
		//if !save
			Init (ThirangConstructor._default);
		//else
			Init (ThirangConstructor._save);
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (berserkIsActive) {
			timer += Time.deltaTime;
			if (timer >= berserkTime) {
				stopBerserk ();
			}
		}

		print (gold + " " + exp);
	}

	void Berserk() {
		berserkIsActive = true;
		//AnimationBerserk();
	}

	void stopBerserk() {
		berserkIsActive = false;
		//
	}

	public void OnCollisionShield (Collider other) {
		
	}

	public void OnCollisionSword (Collider other) {
		if (other.gameObject.CompareTag ("Enemy") && anim.GetBool ("IsFighting"))
		{
			if (newAttack) {
				OnDamage (other.gameObject, currAbility);
				other.gameObject.SendMessage ("Hit");
				newAttack = false;
			}
		}
	}

	public void UpdateValues () {
		attackDamage = attackDamage * (10 * level + 100) / 100;
		berserk.damage = attackDamage * 110 / 100;
		cycloneSpin.damage = attackDamage * 140 / 100;
		magicArrow.damage = attackDamage * 200 / 100;
		berserkTime = 3 * level;
	}
}