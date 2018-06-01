using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryRockFragments : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (WaitDestroy ());
	}
	
	IEnumerator WaitDestroy() {
		yield return new WaitForSecondsRealtime (6f);
		Destroy (this.gameObject);
	}
}
