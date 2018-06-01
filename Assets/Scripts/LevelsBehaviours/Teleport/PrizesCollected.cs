using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizesCollected : MonoBehaviour {
	int prizesToCollect;

	// Use this for initialization
	void Start () {
		GameObject[] prizes = GameObject.FindGameObjectsWithTag ("Prize");
		prizesToCollect = prizes.Length;
	}

	void PrizeCollected() {
		prizesToCollect--;

		if (prizesToCollect == 0)
			AllPrizesCollected ();
	}

	void AllPrizesCollected () {
		GetComponent<TeleportBehaviour> ().enabled = true;
	}

}
