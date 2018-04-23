using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	protected int health;
	protected int mana;
	protected int attackDamage;
	protected int armor;
	protected int magicResist;

	public enum DamageType { physical, magic };

	public struct Ability {
		public int damage;
		public DamageType damageType;

		public Ability(int d, DamageType dT) {
			damage = d;
			damageType = dT;
		}
	}

	public Character(int health, int mana, int attackDamage, int armor, int magicResist) {
		this.health = health;
		this.mana = mana;
		this.attackDamage = attackDamage;
		this.armor = armor;
		this.magicResist = magicResist;
	}

	protected void OnDamage (GameObject hit, Ability a) {
		Character ch = null;
		if (hit.CompareTag ("Player")) {
			ch = GetComponent <Thirang> () as Character;
		} else {	//GetAllComponents of "hit" to find the "enemy" and control which is not null (i.e. assignable to Character class)
			foreach (Character c in hit.GetComponents <Component> ()) {
				ch = (Character) c;
				if (ch != null) {
					break;
				}
			}
		}

		if (ch != null) {
			if (a.damageType == DamageType.physical)
				ch.health = a.damage * armor;

			if (a.damageType == DamageType.magic)
				ch.health = a.damage * magicResist;
		} else {
			throw new System.NullReferenceException ();
		}
	}
}
