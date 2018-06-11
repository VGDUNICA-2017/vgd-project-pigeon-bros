using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirang : Character {
	ThirangController thCtrl;
	AbilitiesController thAbilCtrl;

	int mana { get; set; }
	public int level { get; set; }
	public int gold { get; set; }
	public int exp { get; set; }
	const int TO_LEVEL_2 = 10000;
	const int TO_LEVEL_3 = 80000;

	Ability autoAttack;
	Ability berserk;
	Ability cycloneSpin;
	Ability magicArrow;
	Ability goddessBlessing;
	private Ability currAbility;
	public Ability abilityState { get; set; }

		
	float timer;

	int berserkTime;
	bool berserk_ON, toBerserk, toUnberserk;
	public bool berserkEnd { get; set; }
	float defaultScale = 2.4f;
	float berserkScale = 3.2f;
	public float BerserkScaleMultiplier;

	public int timeGodBlessed { get; set; }

	public bool isDead { get; set; }
	int lastHealthValue;

	public void Init() {
		Vector3 pos;
		pos.x = PlayerPrefs.GetFloat ("xP", transform.position.x);
		pos.y = PlayerPrefs.GetFloat ("yP", transform.position.y);
		pos.z = PlayerPrefs.GetFloat ("zP", transform.position.z);
		transform.position = pos;

		health = PlayerPrefs.GetInt("health", 12000);
		mana = PlayerPrefs.GetInt("mana", 300);
		attackDamage = PlayerPrefs.GetInt("attackDamage", 100);
		armor = PlayerPrefs.GetInt("armor", 25);
		magicResist = PlayerPrefs.GetInt("magicResist", 24);
		level = PlayerPrefs.GetInt("level", 1);

		if (level == 1) {
			autoAttack = new Ability (attackDamage, DamageType.physical);
			berserk = new Ability (attackDamage * 110 / 100, DamageType.physical);
			cycloneSpin = new Ability (attackDamage * 140 / 100, DamageType.physical);
			magicArrow = new Ability (attackDamage * 200 / 100, DamageType.magic);
			goddessBlessing = new Ability (0, DamageType.invulnerable);
			berserkTime = 4 + (3 * level);
			timeGodBlessed = 4;
		} else
			UpdateValues ();

	}

	void Awake() {
		Enemy.LoadEnemies ();

		Init ();
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

		if (exp >= TO_LEVEL_2 && level < 2) {
			level = 2;
			UpdateValues ();
		}

		if (exp >= TO_LEVEL_3 && level < 3) {
			level = 3;
			UpdateValues ();
		}

		print (health);
	}

	void LateUpdate() {
		if (toBerserk)
			BerserkAnimation (1);

		if (toUnberserk)
			BerserkAnimation (-1);
	}

	public void SaveThirangData(bool savePosition) {
		if (savePosition) {
			PlayerPrefs.SetFloat ("xP", transform.position.x);
			PlayerPrefs.SetFloat ("yP", transform.position.y);
			PlayerPrefs.SetFloat ("zP", transform.position.z);
		} else {
			//Just one is necessary to know Thirang's position was been saved
			if (PlayerPrefs.HasKey ("xP")) {
				PlayerPrefs.DeleteKey ("xP");
				PlayerPrefs.DeleteKey ("yP");
				PlayerPrefs.DeleteKey ("zP");
			}
		}
		PlayerPrefs.SetInt ("health", health);
		PlayerPrefs.SetInt ("mana", mana);
		PlayerPrefs.SetInt ("attackDamage", attackDamage);
		PlayerPrefs.SetInt ("armor", armor);
		PlayerPrefs.SetInt ("magicResist", magicResist);
		PlayerPrefs.SetInt ("level", level);

		PlayerPrefs.Save ();
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
		health = health * (10 * level + 100) / 100;
		mana = mana * (10 * level + 100) / 100;
		attackDamage = attackDamage * (10 * level + 100) / 100;
		armor = armor * (10 * level + 100) / 100;
		magicResist = magicResist * (10 * level + 100) / 100;

		berserk.damage = attackDamage * 110 / 100;
		cycloneSpin.damage = attackDamage * 140 / 100;
		magicArrow.damage = attackDamage * 200 / 100;
		berserkTime = 4 + (3 * level);
		timeGodBlessed = timeGodBlessed + level;
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
				abilityState = goddessBlessing;
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