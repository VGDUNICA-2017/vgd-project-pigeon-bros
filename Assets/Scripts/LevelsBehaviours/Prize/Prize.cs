using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Prize : MonoBehaviour {
	public string powerUp;
	public AudioClip audioClip;

	bool powerUpApplied;

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !powerUpApplied) {
			//AudioSource.PlayClipAtPoint (audioClip, transform.position);
			powerUpApplied = true;
			FindObjectOfType<PrizesCollected> ().SendMessage ("PrizeCollected");

			PowerUp (other.transform.root.gameObject.GetComponent<Thirang> ());

			Destroy (this.gameObject);
		}
	}

	public void PowerUp (Thirang th) {
		switch (powerUp) {
			case "health":
				th.health += 400;
				break;
			case "attackDamage":
				th.attackDamage += 40;
				break;
			case "armor":
				th.armor += 20;
				break;
			case "magicResist":
				th.magicResist += 18;
				break;
			default:
				break;
		}
	}
}
