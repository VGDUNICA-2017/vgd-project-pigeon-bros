using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	/*Number of hits to kill
	 * PhysicalHit = damage * 100 / armorHit;
	 * MagicHit = damage * 100 / magicResitHit;
	 * nHits = health / (Physical\Magic)Hit;
	 * 
	 * #if defense = magicResist || armor;
	 * 
	 * #then nHits = (health * defense) / (damage * 100);
	 * #then damage = (health * defense) / (nHits * 100);
	 * #then defense = (damage * nHits * 100) / health
	 */

	public int health { get; set; }
	public int attackDamage { get; set; }
	public int armor { get; set; }
	public int magicResist { get; set; }

	public enum DamageType { none, physical, magic, invulnerable, _true };

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
		bool thirangInvulnerable = false;

		if (hit.CompareTag ("Player")) {
			ch = hit.GetComponent <Thirang> () as Character;

			if (((Thirang)ch).abilityState.damageType == DamageType.invulnerable)
				thirangInvulnerable = true;

		} else {
			ch = hit.GetComponent <Enemy> () as Character;
		}
			
		if (ch != null) {
			
			if (ch.health > 0 && !thirangInvulnerable) {
				if (a.damageType == DamageType.physical)
					ch.health -= a.damage * 100 / ch.armor;

				if (a.damageType == DamageType.magic)
					ch.health -= a.damage * 100 / ch.magicResist;

				if (a.damageType == DamageType._true)
					ch.health -= a.damage;
			}

		} else {
			throw new System.NullReferenceException ();
		}
	}
}
