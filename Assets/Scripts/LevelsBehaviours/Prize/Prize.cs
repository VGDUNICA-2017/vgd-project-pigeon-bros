using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Prize : MonoBehaviour {
	public string powerUp;
	//public AudioClip audioClip;

	bool powerUpApplied;

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !powerUpApplied) {
			powerUpApplied = true;
			FindObjectOfType<PrizesCollected> ().SendMessage ("PrizeCollected");

			PowerUp (other.transform.root.GetComponent<Thirang> ());

			foreach (Transform child in transform) {
				Destroy (child.gameObject);
			}

			StartCoroutine (DestroyWait ());
		}
	}

	public void PowerUp (Thirang th) {
		switch (powerUp) {
			case "health":
				th.maxHealth += 400;
				break;
			case "mana":
				th.maxMana += 40;
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

	IEnumerator DestroyWait() {
		yield return new WaitForSecondsRealtime (25f);
		Destroy (this.gameObject);
	}
}
