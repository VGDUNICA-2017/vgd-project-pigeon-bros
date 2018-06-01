using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBossFight : MonoBehaviour {
	Animator anim, animBoss;
	public GameObject boss;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		//animBoss = boss.GetComponent<Animator> ();

		StartCoroutine (OnBoss ());
	}

	IEnumerator OnBoss () {
		yield return new WaitForSecondsRealtime (1.5f);
		anim.SetTrigger ("OnBoss");

		//animBoss animation start;
		/*ON BOSS ANIMATION END*/
		yield return new WaitForSecondsRealtime (3.5f);
		anim.SetTrigger ("OnLevel");
	}
}
