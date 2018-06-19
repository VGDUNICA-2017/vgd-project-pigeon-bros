using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirang : Character {
	ThirangController thCtrl;
	AbilitiesController thAbilCtrl;

	public int maxHealth { get; set; }
	public int mana { get; set; }
	public int maxMana { get; set; }
	public int level { get; set; }
	public int gold { get; set; }
	public int exp { get; set; }
	public int hpPotions { get; set; }
	public int mpPotions { get; set; }
	const int TO_LEVEL_2 = 5000;
	const int TO_LEVEL_3 = 10000;

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

	public bool onFight { get; set; }
	public bool onJump { get; set; }
	public bool onMovingGround { get; set; }
	public bool isDead { get; set; }
	public bool onGround { get; set; }
	int lastHealthValue;

	//Need to control the Raycast from body and not feet
	public bool onEarthBossFight { get; set; }

	public void Init() {
		Vector3 pos;
		pos.x = PlayerPrefs.GetFloat ("xP", transform.position.x);
		pos.y = PlayerPrefs.GetFloat ("yP", transform.position.y);
		pos.z = PlayerPrefs.GetFloat ("zP", transform.position.z);
		transform.position = pos;

		maxHealth = PlayerPrefs.GetInt ("maxHealth", 12000);
		health = PlayerPrefs.GetInt ("health", 12000);
		maxMana = PlayerPrefs.GetInt ("maxMana", 300);
		mana = PlayerPrefs.GetInt ("mana", 300);
		attackDamage = PlayerPrefs.GetInt ("attackDamage", 100);
		armor = PlayerPrefs.GetInt ("armor", 25);
		magicResist = PlayerPrefs.GetInt ("magicResist", 24);
		hpPotions = PlayerPrefs.GetInt ("hpPotions", 3);
		mpPotions = PlayerPrefs.GetInt ("mpPotions", 3);
		level = PlayerPrefs.GetInt ("level", 1);

		gold = PlayerPrefs.GetInt ("gold", 0);
		exp = PlayerPrefs.GetInt ("exp", 0);

		autoAttack = new Ability (attackDamage, DamageType.physical);
		berserk = new Ability (attackDamage * 110 / 100, DamageType.physical);
		cycloneSpin = new Ability (attackDamage * 140 / 100, DamageType.physical);
		magicArrow = new Ability (attackDamage * 200 / 100, DamageType.magic);
		goddessBlessing = new Ability (0, DamageType.invulnerable);
		berserkTime = 4 + (3 * level);
		timeGodBlessed = 4 + level;

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

		PlayerPrefs.SetString ("Scene", UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name);
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

		if (health > maxHealth)
			health = maxHealth;

		if (mana > maxMana)
			mana = maxMana;

		if (Input.GetButtonDown ("HP")) {
			if (hpPotions > 0) {
				hpPotions--;
				health += 4000;
			}
		}

		if (Input.GetButtonDown ("MP")) {
			if (mpPotions > 0) {
				mpPotions--;
				mana += 100;
			}
		}

		if (isDead) {
			StartCoroutine(WaitDeath ());
		}
	}

	public bool SpecialAbility (string ability) {
		switch (ability) {
			case "destroyRock":
				if (level > 1)
					return true;
				break;
			case "superJump":
				if (level > 2)
					return true;
				break;
		}

		return false;
	}

	public static void NewGame() {
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetInt ("gameLevel", 1);
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
		PlayerPrefs.SetInt ("maxHealth", maxHealth);
		PlayerPrefs.SetInt ("health", health);
		PlayerPrefs.SetInt ("maxMana", maxMana);
		PlayerPrefs.SetInt ("mana", mana);
		PlayerPrefs.SetInt ("attackDamage", attackDamage);
		PlayerPrefs.SetInt ("armor", armor);
		PlayerPrefs.SetInt ("magicResist", magicResist);
		PlayerPrefs.SetInt ("level", level);
		PlayerPrefs.SetInt ("gold", gold);
		PlayerPrefs.SetInt ("exp", exp);
		PlayerPrefs.SetInt ("hpPotions", hpPotions);
		PlayerPrefs.SetInt ("mpPotions", mpPotions);

		PlayerPrefs.Save ();
	}

	public KeyValuePair<bool, string> CanSave() {
		if (!isDead && !onFight && !onJump && !onMovingGround && onGround)
			return new KeyValuePair<bool, string> (true, "");

		if (isDead)
			return new KeyValuePair<bool, string> (false, "Can't save if you're dead");

		if (onFight)
			return new KeyValuePair<bool, string> (false, "Can't save during fight");

		if (onJump)
			return new KeyValuePair<bool, string> (false, "Can't save while jumping");

		if (onMovingGround)
			return new KeyValuePair<bool, string> (false, "Can't save while on MovingGround");

		throw new UnityException ();
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
		maxHealth += maxHealth * level / 100;
		maxMana += maxMana * level / 100;
		attackDamage += attackDamage * level / 100;
		armor += armor * level / 100;
		magicResist += magicResist * level / 100;

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

	public void BuyObject(string type, int value) {
		switch (type) 
		{
		case "hpPotion": 
			hpPotions += value;
			break;
		case "mpPotion":
			mpPotions += value;
			break;
		case "hp":
			maxHealth += value;
			break;
		case "mp":
			maxMana += value;
			break;
		case "atkUp":
			attackDamage += value;
			break;
		case "defUp":
			magicResist += value;
			armor += value;
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
		Ability fireBall = new Ability (this.maxHealth * 10 / 100, DamageType._true);
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

	IEnumerator WaitDeath() {
		yield return new WaitForSecondsRealtime (2f);
		gameObject.AddComponent<MainMenu> ().Load ();
	}
}