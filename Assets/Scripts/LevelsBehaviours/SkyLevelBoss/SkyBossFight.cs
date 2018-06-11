using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBossFight : MonoBehaviour {
	Animator anim;
	public GameObject boss;
	public GameObject bossGround;
	GameObject thirang;

	void Awake() {
		thirang = GameObject.FindGameObjectWithTag ("Player");
		thirang.GetComponent<ThirangController> ().enabled = false;
		Vector3 rot = thirang.transform.eulerAngles;
		rot.y = 90f;
		thirang.transform.eulerAngles = rot;

	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		StartCoroutine (OnBoss ());
	}

	IEnumerator OnBoss () {
		yield return new WaitForSecondsRealtime (1.5f);
		anim.SetTrigger ("OnBoss");

		yield return new WaitForSecondsRealtime (3.5f);
		anim.SetTrigger ("OnLevel");
		yield return new WaitForSecondsRealtime (3.5f);
		thirang.GetComponent<ThirangController> ().enabled = true;
		boss.GetComponent<MageController> ().enabled = true;
		bossGround.GetComponent<GroundBoss> ().enabled = true;
	}
}
