using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy {
	OrcController orcCtrl;

	Ability autoAttack;

	public bool fadingDeath { get; set; }
	float deathTimer;
	Renderer[] gosRends;

	void Awake() {
		this.health = 20;
		this.attackDamage = 1;
		this.armor = 10;
		this.magicResist = 10;
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
		this.isAttacking = orcCtrl.isAttacking;

		if (!this.isAttacking)
			this.justAttacked = false;

		if (fadingDeath)
			FadeDeathOrc ();

		ReadyNewAttack ();
	}

	void OnTriggerEnter (Collider other) {
		TriggerEnter (other);
	}
	

	public void OnAttack (Collider other) {
		DealDamage (other, autoAttack);
	}

	public void OnDeath() {
		base.OnDeath (gold: 50, exp: 100);
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
	
	protected override bool ThirangFacingEnemy () {
		return (thirang.FacingRight() && orcCtrl.isFacingLeft) || (!thirang.FacingRight() && !orcCtrl.isFacingLeft);
	}
}