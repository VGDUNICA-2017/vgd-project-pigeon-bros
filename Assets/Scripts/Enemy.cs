using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {

	protected Thirang thirang;

	private static GameObject _orc, _archer;

	public enum EnemyType { Orc, Archer };

	[Range(0f,1f)]
	public float hitAnimationProbability;

	protected EnemyType enemyType;
	protected bool hit;
	protected bool justAttacked;
	protected bool isAttacking;

	public bool fadingDeath { get; set; }
	protected float deathTimer;

	public static void LoadEnemies() {
		_orc = Resources.Load<GameObject> ("Orc");
		_archer = Resources.Load<GameObject> ("Archer");
	}

	public static GameObject InstantiateEnemy (EnemyType e) {
		switch (e) {
			case EnemyType.Orc:
				return Instantiate (_orc);
			case EnemyType.Archer:
				return Instantiate (_archer);
			default:
				throw new System.ArgumentException ("Cannot find enemy with this type");
		}
	}

	public static GameObject InstantiateEnemy (EnemyType enemyType, Vector3 position, Quaternion rotation, bool isFacingLeft) {
		GameObject g;

		switch (enemyType) {
			case EnemyType.Orc:
				g = Instantiate (_orc, position, rotation);
				g.GetComponent<OrcController> ().isFacingLeft = isFacingLeft;
				return g;
			case EnemyType.Archer:
				g = Instantiate (_archer, position, rotation);
				g.GetComponent<ArcherController> ().isFacingLeft = isFacingLeft;
				return g;
			default:
				throw new System.ArgumentException ("Cannot find enemy with this type");
		}
	}

	protected void OnDeath(int gold, int exp, int health, int mana) {
		thirang.gold += gold;
		thirang.exp += exp;
		thirang.health += health;
		thirang.mana += mana;
	}

	public void Hit() {
		if (this.health > 0) {
			this.gameObject.GetComponent<Animator> ().SetTrigger ("Hit");
		}
	}

	protected void TriggerEnter (Collider other) {
		if (!hit && (other.CompareTag ("Sword") && thirang.Fighting ()) || other.CompareTag ("Arrow")) {
			if (thirang.OnSlash2 ())
				StartCoroutine (DamagedWithWait ());
			else
				Damaged (this.gameObject);

			hit = true;
		}
	}

	protected void TriggerEnter (Collider other, bool hitAnimation) {
		if (!hit && (other.CompareTag ("Sword") && thirang.Fighting ()) || other.CompareTag ("Arrow")) {
			if (thirang.OnSlash2 ())
				StartCoroutine (DamagedWithWait ());
			else
				Damaged (this.gameObject, hitAnimation);

			hit = true;
		}
	}

	IEnumerator DamagedWithWait() {
		yield return new WaitForSeconds (0.5f);
		Damaged (this.gameObject);
	}

	protected void Damaged(GameObject g) {
		if (ThirangFacingEnemy ()) {
			OnDamage (g, thirang.GetCurrentAbility ());

			int rand = Random.Range (1, 101);
			bool hitAnimation = rand >= 1 && rand <= (int)(hitAnimationProbability * 100);
			if (hitAnimation)
				Hit ();
		}
	}

	protected void Damaged(GameObject g, bool hitAnimation) {
		if (ThirangFacingEnemy ()) {
			OnDamage (g, thirang.GetCurrentAbility ());
			if(!hitAnimation)
				Hit ();
		}
	}

	private bool CanDealDamageFighter (Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			if (!this.justAttacked) {
				if ( !(thirang.GetComponent<Animator> ().GetBool ("IsShielding") && ThirangEnemyFacingEachOther ()) )
					return true;
				else
					return false;
			} else {
				return false;
			}
		}

		return false;
	}

	protected void DealDamageFighter (Collider other, Ability ability) {
		if (isAttacking) {
			if (CanDealDamageFighter (other)) {
				OnDamage (other.transform.root.gameObject, ability);
				justAttacked = true;
			}
		} else {
			justAttacked = false;
		}
	}

	protected void DealDamageFighter (Collider other, Ability[] abilities) {
		if (isAttacking) {
			if (CanDealDamageFighter (other)) {
				foreach (var a in abilities) {
					OnDamage (other.transform.root.gameObject, a);
				}
				justAttacked = true;
			}
		} else {
			justAttacked = false;
		}
	}

	protected void DealDamageCaster (Collider other, Ability ability) {
		if (CanDealDamageCaster (other)) {
			OnDamage (other.transform.root.gameObject, ability);
		}
	}

	private bool CanDealDamageCaster (Collider other) {
		if ( !(thirang.GetComponent<Animator> ().GetBool ("IsShielding") && ThirangEnemyFacingEachOther ()) )
			return true;
		else
			return false;
	}

	protected void ReadyNewAttack_ThirangAlive() {
		if (thirang.ChangingState())
			hit = false;	//Thirang's animation state changed, so a new attack can be ready to hit the enemy

		if (thirang.isDead) {
			EnemyController e = this.GetComponent<EnemyController> ();
			e.enabled = false;
			this.GetComponent<Animator> ().Play (e.isFacingLeft == true ? EnemySaT.idleStateHash : EnemySaT.idleBackStateHash);
		}
	}
	
	protected abstract bool ThirangEnemyFacingEachOther ();

	bool ThirangFacingEnemy() {
		ThirangController thCtrl = thirang.GetComponent<ThirangController> ();

		return (thCtrl.isFacingRight && this.transform.position.x > thirang.transform.position.x) || 
			   (!thCtrl.isFacingRight && this.transform.position.x < thirang.transform.position.x);
	}

	public bool ThirangOnCycloneSpin() {
		return thirang.OnCycloneSpin();
	}

	public void FadeDeath (Renderer[] gosRends) {
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
	protected void changeRenderingMode (Material m) {
		m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		m.SetInt("_ZWrite", 0);
		m.DisableKeyword("_ALPHATEST_ON");
		m.EnableKeyword("_ALPHABLEND_ON");
		m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		m.renderQueue = 3000;
	}
}
