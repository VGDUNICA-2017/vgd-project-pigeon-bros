using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirang : MonoBehaviour {
	int health;
	int mana;
	int attackDamage;
	int armor;
	int magicResist;
	int manaRegen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Berserk() {
		attackDamage += attackDamage * 20 / 100;
	}

	public void OnCollisionEvent (Collision other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			
		}
	}

	public void OnCollisionShield (Collision other) {
		
	}

	public void OnCollisionSword (Collision other) {
		
	}
}
