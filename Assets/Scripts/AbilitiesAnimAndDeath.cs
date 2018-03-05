using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesAnimAndDeath : Thirang_Controller {
	private Animator anim;
	bool godBlessed;
	float timeGodBlessed;
	GameObject godLight;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		timeGodBlessed = 0;
		godLight = GameObject.Find ("Goddess' Blessing Light");
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		//Berserk
		if ((stateInfo.fullPathHash == idleStateHash || stateInfo.fullPathHash == backIdleStateHash) &&
			Input.GetButtonDown ("Berserk")) 
		{
			anim.SetTrigger ("Berserk");
		}

		//Cyclone Spin
		if ((stateInfo.fullPathHash == idleStateHash || stateInfo.fullPathHash == backIdleStateHash) &&
			Input.GetButtonDown ("Cyclone Spin")) 
		{
			anim.SetTrigger ("Cyclone Spin");
		}

		//Magic Arrow
		if ((stateInfo.fullPathHash == idleStateHash || stateInfo.fullPathHash == backIdleStateHash) &&
			Input.GetButtonDown ("Magic Arrow")) 
		{
			anim.SetTrigger ("Magic Arrow");
		}

		//Goddess' Blessing

		if (Input.GetButtonDown ("Goddess' Blessing")) {
			godLight.GetComponent<Light> ().enabled = true;
			godBlessed = true;
		}

		if (godBlessed) {
			timeGodBlessed += Time.deltaTime;
			if (timeGodBlessed >= 4f) {
				godLight.GetComponent<Light> ().enabled = false;
				godBlessed = false;
				timeGodBlessed = 0;
			}
		}
	}
}
