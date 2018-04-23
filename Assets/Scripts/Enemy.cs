using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	public GameObject orc;
	private static GameObject _orc;
	public enum EnemyType { Orc };

	protected EnemyType enemyType;

	public Enemy(int health, int mana, int attackDamage, int armor, int magicResist)
		:base(health, mana, attackDamage, armor, magicResist) {
	
	}

	void Start() {
		_orc = orc;
	}

	public void _instantiateEnemy (EnemyType e) {
		switch (e) {
			case EnemyType.Orc:
				Instantiate (orc);
				break;
			default:
				throw new System.ArgumentException ("Cannot find enemy with this type");
		}
	}
}
