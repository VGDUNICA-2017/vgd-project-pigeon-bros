using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour {
	Animator anim;
	GameObject thirang;

	public float distOrcPlayer;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if ((transform.position - thirang.transform.position).magnitude < distOrcPlayer) {
			anim.SetTrigger ("Attack");
		}
	}
}
