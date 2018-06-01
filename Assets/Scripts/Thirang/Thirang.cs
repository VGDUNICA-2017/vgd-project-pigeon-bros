using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirang : Character {
	ThirangController thCtrl;
	AbilitiesController thAbilCtrl;

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
	Ability goddessBlessing;
	private Ability currAbility;
	int berserkTime;
		
	float timer;
	bool berserk_ON, toBerserk, toUnberserk;
	public bool berserkEnd { get; set; }
	float defaultScale = 2.4f;
	float berserkScale = 3.2f;
	public float BerserkScaleMultiplier;

	public bool isDead { get; set; }
	int lastHealthValue;

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
				cycloneSpin = new Ability (attackDamage * 140 / 100, DamageType.physical);
				magicArrow = new Ability (attackDamage * 200 / 100, DamageType.magic);
				goddessBlessing = new Ability (0, DamageType.invulnerable);
				berserkTime = 4 + (3 * level);
				break;
			case ThirangConstructor._save:
				break;
			default:
				throw new System.ArgumentException ("Error initializing Thirang");
		}
	}

	void Awake() {
		Enemy.LoadEnemies ();

		//if !save
			Init (ThirangConstructor._default);
		//else
			Init (ThirangConstructor._save);
	}

	// Use this for initialization
	void Start () {
		thCtrl = GetComponent<ThirangController> ();
		thAbilCtrl = GetComponent<AbilitiesController> ();
		timer = 0;

		lastHealthValue = this.health;
		berserkEnd = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (berserk_ON) {
			timer += Time.deltaTime;
			if (timer >= berserkTime) {
				stopBerserk ();
			}
		}

		print (health);
	}

	void LateUpdate() {
		if (toBerserk)
			BerserkAnimation (1);

		if (toUnberserk)
			BerserkAnimation (-1);
	}

	public void Berserk() {
		berserkEnd = false;
		berserk_ON = true;
		toBerserk = true;
	}

	void BerserkAnimation (int speed) {
		if (transform.localScale.x >= defaultScale && transform.localScale.x <= berserkScale) {
			transform.localScale += speed * new Vector3 (BerserkScaleMultiplier, BerserkScaleMultiplier, BerserkScaleMultiplier);
		} else if (transform.localScale.x < defaultScale) {
			transform.localScale = new Vector3 (defaultScale, defaultScale, defaultScale);
			toUnberserk = false;
			currAbility = autoAttack;
			berserkEnd = true;
		} else {
			transform.localScale = new Vector3 (berserkScale, berserkScale, berserkScale);
			toBerserk = false;
		}
	}

	void stopBerserk() {
		berserk_ON = false;
		timer = 0;
		toUnberserk = true;
	}

	public void UpdateValues () {
		attackDamage = attackDamage * (10 * level + 100) / 100;
		berserk.damage = attackDamage * 110 / 100;
		cycloneSpin.damage = attackDamage * 140 / 100;
		magicArrow.damage = attackDamage * 200 / 100;
		berserkTime = 4 + (3 * level);
	}

	public Ability GetCurrentAbility() {
		return currAbility;
	}

	public void SetCurrentAbility(string ability) {
		switch (ability) 
		{
			case "autoAttack": 
				if (currAbility.damage != berserk.damage)	//if Thirang is on Berserk State currAbility remains = berserk
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
			case "goddessBlessing":
				currAbility = goddessBlessing;
				break;
			default:
				throw new System.ArgumentException ("Cannot find ability with this name");
		}
	}

	public void FatalDamage () {
		Ability fatal = new Ability (this.health, DamageType._true);
		thCtrl.deathTrap = true;	//death cause not only by traps
		OnDamage (this.gameObject, fatal);
	}

	public void FireBallDamage() {
		Ability fireBall = new Ability (this.health * 10 / 100, DamageType._true);
		OnDamage (this.gameObject, fireBall);
	}

	public bool Fighting () {
		return thCtrl.isFighting;
	}

	public bool ChangingState() {
		return thCtrl.changingState;
	}

	public bool OnSlash2() {
		return thCtrl.onSlash2;
	}
	
	public bool OnCycloneSpin() {
		return thAbilCtrl.onCycloneSpin;
	}
	
	public bool FacingRight() {
		return thCtrl.isFacingRight;
	}

	public bool BeenAttacked() {
		if (this.health != lastHealthValue) {
			lastHealthValue = this.health;
			return true;
		}

		return false;
	}
}