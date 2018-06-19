﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantBehaviour : MonoBehaviour {
	public GameObject UI;
	public GameObject menuStore;

	public static bool onStore;
	bool firstWait = true;

	void OnTriggerStay(Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			if (Input.GetButtonDown ("Merchant")) {
				if (!onStore) {
					OpenMenuStore ();
				}
			}
		}
	}

	void Update() {
		if (onStore) {
			StartCoroutine (Wait ());
		}
	}

	void OpenMenuStore () {
		onStore = true;
		UI.SetActive (false);
		menuStore.SetActive (true);
		Time.timeScale = 0f;
	}
		
	public void CloseMenuStore () {
		onStore = false;
		UI.SetActive (true);
		menuStore.SetActive (false);
		Time.timeScale = 1f;
		firstWait = true;
	}

	IEnumerator Wait() {
		if (firstWait) {
			yield return new WaitForSecondsRealtime (0.5f);
			firstWait = false;
		}
		if (Input.GetButtonDown ("Merchant"))
			CloseMenuStore ();
	}
}