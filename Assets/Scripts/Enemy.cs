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

	public void Hit() {
		if (this.health > 0) {
			this.gameObject.GetComponent<Animator> ().SetTrigger ("Hit");
		}
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
	
	protected abstract bool ThirangFacingEnemy ();
}
