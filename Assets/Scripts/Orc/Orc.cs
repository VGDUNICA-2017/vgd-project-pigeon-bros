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
	Renderer[] gosRends;

	void Awake() {
		this.health = 20;
		this.attackDamage = 1;
		this.armor = 10;
		this.magicResist = 10;
		this.expAssigned = 100;
		this.goldAssigned = 50;
		this.enemyType = EnemyType.Orc;
		this.autoAttack = new Ability (25, DamageType.physical);
	}

	// Use this for initialization
	void Start () {
		orcCtrl = GetComponent<OrcController> ();
		gosRends = GetComponentsInChildren<Renderer> ();

		thirang = FindObjectOfType<Thirang> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!orcCtrl.isAttacking)
			attacked = false;

		if (fadingDeath)
			FadeDeathOrc ();

		if (thirang.ChangingState())
			hit = false;	//Thirang's animation state changed, so a new attack can be ready to hit the enemy
	}

	void OnTriggerEnter (Collider other) {
		if (!hit &&
			( (other.CompareTag ("Sword") && thirang.Fighting()) || other.CompareTag ("Arrow")) ) 
		{
			if (thirang.OnSlash2 ())
				StartCoroutine(DamagedWithWait ());
			else
				Damaged (this.gameObject);
			
			hit = true;
		}
	}

	IEnumerator DamagedWithWait() {
		yield return new WaitForSeconds (0.5f);
		Damaged (this.gameObject);
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
		thirang.gold = goldAssigned;
		thirang.exp = expAssigned;
	}

	public void FadeDeathOrc() {
		if (deathTimer <= 0f) {
			foreach (Renderer r in gosRends) {
				changeRenderingMode (r.material);
			}
		}
		deathTimer += Time.deltaTime;
		if (deathTimer >= 2f) {
			foreach (Renderer r in gosRends) {
				if (r.material.color.a >= 0f) {
					Color newColor = r.material.color;
					newColor.a -= 0.01f;
					r.material.SetColor ("_Color", newColor);
				} else {
					Destroy (this.gameObject);
				}
			}
		}

	}

	//https://answers.unity.com/questions/1004666/change-material-rendering-mode-in-runtime.html
	void changeRenderingMode (Material m) {
		m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		m.SetInt("_ZWrite", 0);
		m.DisableKeyword("_ALPHATEST_ON");
		m.EnableKeyword("_ALPHABLEND_ON");
		m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		m.renderQueue = 3000;
	}
}