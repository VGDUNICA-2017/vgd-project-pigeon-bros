using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	protected Thirang thirang;

	[SerializeField] private GameObject orc;
	[SerializeField] private GameObject mage;

	private static GameObject _orc;
	public enum EnemyType { Orc };

	protected EnemyType enemyType;
	protected bool hit;

	void Start() {
		_orc = orc;
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

	public void Hit() {
		if (this.health > 0) {
			this.gameObject.GetComponent<Animator> ().SetTrigger ("Hit");
		}
	}

	protected void Damaged(GameObject g) {
		OnDamage (g, thirang.GetCurrentAbility());
		Hit ();
	}
}
