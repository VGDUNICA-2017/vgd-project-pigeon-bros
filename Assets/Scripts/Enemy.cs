using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {

	protected Thirang thirang;

	private static GameObject _orc;

	public enum EnemyType { Orc };

	protected EnemyType enemyType;
	protected bool hit;
	protected bool justAttacked;
	protected bool isAttacking;

	public static void LoadEnemies() {
		_orc = Resources.Load<GameObject> ("Orc");
	}

	public static void instantiateEnemy (EnemyType e) {
		switch (e) {
			case EnemyType.Orc:
				Instantiate (_orc);
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
			default:
				throw new System.ArgumentException ("Cannot find enemy with this type");
		}
	}

	protected void OnDeath(int gold, int exp) {
		thirang.gold = gold;
		thirang.exp = exp;
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

	IEnumerator DamagedWithWait() {
		yield return new WaitForSeconds (0.5f);
		Damaged (this.gameObject);
	}

	protected void Damaged(GameObject g) {
		if (ThirangFacingEnemy ()) {
			OnDamage (g, thirang.GetCurrentAbility ());
			Hit ();
		}
	}

	protected bool CanDealDamage (Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			if (!this.justAttacked) {
				return true;
			} else {
				return false;
			}
		}

		return false;
	}

	protected void DealDamage (Collider other, Ability ability) {
		if (isAttacking) {
			if (CanDealDamage (other)) {
				OnDamage (other.transform.root.gameObject, ability);
				justAttacked = true;
			}
		} else {
			justAttacked = false;
		}
	}

	protected void ReadyNewAttack() {
		if (thirang.ChangingState())
			hit = false;	//Thirang's animation state changed, so a new attack can be ready to hit the enemy
	}
	
	protected abstract bool ThirangFacingEnemy ();

	public bool ThirangOnCycloneSpin() {
		return thirang.OnCycloneSpin();
	}
}
