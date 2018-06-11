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

	public static void instantiateEnemy (EnemyType e) {
		switch (e) {
			case EnemyType.Orc:
				Instantiate (_orc);
				break;
			case EnemyType.Archer:
				Instantiate (_archer);
				break;
			default:
				throw new System.ArgumentException ("Cannot find enemy with this type");
		}
	}

	public static void instantiateEnemy (EnemyType enemyType, Vector3 position, Quaternion rotation) {
		switch (enemyType) {
			case EnemyType.Orc:
				Instantiate (_orc, position, rotation);
				break;
			case EnemyType.Archer:
				Instantiate (_archer, position, rotation);
				break;
			default:
				throw new System.ArgumentException ("Cannot find enemy with this type");
		}
	}

	protected void OnDeath(int gold, int exp) {
		thirang.gold += gold;
		thirang.exp += exp;
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
		if (ThirangFacingEnemy () || thirang.transform.position.x > this.transform.position.x) {
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
				if ( !(thirang.GetComponent<Animator> ().GetBool ("IsShielding") && ThirangFacingEnemy ()) )
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

	protected void DealDamageCaster (Collider other, Ability ability) {
		if (CanDealDamageCaster (other)) {
			OnDamage (other.transform.root.gameObject, ability);
		}
	}

	private bool CanDealDamageCaster (Collider other) {
		if ( !(thirang.GetComponent<Animator> ().GetBool ("IsShielding") && ThirangFacingEnemy ()) )
			return true;
		else
			return false;
	}

	protected void ReadyNewAttack() {
		if (thirang.ChangingState())
			hit = false;	//Thirang's animation state changed, so a new attack can be ready to hit the enemy
	}
	
	protected abstract bool ThirangFacingEnemy ();

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
