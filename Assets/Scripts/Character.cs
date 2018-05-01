using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public int health { get; set; }
	public int attackDamage { get; set; }
	public int armor { get; set; }
	public int magicResist { get; set; }

	public enum DamageType { physical, magic };

	public struct Ability {
		public int damage;
		public DamageType damageType;

		public Ability(int d, DamageType dT) {
			damage = d;
			damageType = dT;
		}
	}

	protected void OnDamage (GameObject hit, Ability a) {
		Character ch = null;
		if (hit.CompareTag ("Player")) {
			ch = hit.GetComponent <Thirang> () as Character;
		} else {	//GetAllComponents of "hit" to find the "enemy" and control which is not null (i.e. assignable to Character class)
			ch = hit.GetComponent <Enemy> () as Character;
		}

		print (hit.tag + " " + ch.health + " " + ch.armor + " " + a.damage + " " + ch);
		if (ch != null) {
			if (ch.health > 0) {
				if (a.damageType == DamageType.physical)
					ch.health -= a.damage * ch.armor / 100;

				if (a.damageType == DamageType.magic)
					ch.health -= a.damage * ch.magicResist / 100;
			}
		} else {
			throw new System.NullReferenceException ();
		}
	}
}
