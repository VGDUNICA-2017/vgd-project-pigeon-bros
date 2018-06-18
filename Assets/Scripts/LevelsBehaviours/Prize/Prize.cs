using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Prize : MonoBehaviour {
	public string powerUp;

	void Start() {
		if (PlayerPrefs.GetInt ("gameLevel") == 4)
			PrizeGotYet ();
	}

	bool powerUpApplied;

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !powerUpApplied) {
			powerUpApplied = true;
			if (FindObjectOfType<PrizesCollected> ())
				FindObjectOfType<PrizesCollected> ().SendMessage ("PrizeCollected");

			PowerUp (other.transform.root.GetComponent<Thirang> ());
			PrizeJustGot ();

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

	void PrizeGotYet() {
		switch (powerUp) {
			case "health":
				if (PlayerPrefs.HasKey ("healthPrize")) {
					FindObjectOfType<PrizesCollected> ().SendMessage ("PrizeCollected");
					Destroy (this.gameObject);
				}
				break;
			case "mana":
				if (PlayerPrefs.HasKey ("manaPrize")) {
					FindObjectOfType<PrizesCollected> ().SendMessage ("PrizeCollected");
					Destroy (this.gameObject);
				}
				break;
			case "armor":
				if (PlayerPrefs.HasKey ("armorPrize")) {
					FindObjectOfType<PrizesCollected> ().SendMessage ("PrizeCollected");
					Destroy (this.gameObject);
				}
				break;
			case "magicResist":
				if (PlayerPrefs.HasKey ("magicResistPrize")) {
					FindObjectOfType<PrizesCollected> ().SendMessage ("PrizeCollected");
					Destroy (this.gameObject);
				}
				break;	
			default:
				break;
		}
	}

	void PrizeJustGot () {
		switch (powerUp) {
		case "health":
			PlayerPrefs.SetString ("healthPrize", "");
			break;
		case "mana":
			PlayerPrefs.SetString ("manaPrize", "");
			break;
		case "armor":
			PlayerPrefs.SetString ("armorPrize", "");
			break;
		case "magicResist":
			PlayerPrefs.SetString ("magicResistPrize", "");
			break;	
		default:
			break;
		}
	}
}
