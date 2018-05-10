using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherAnimations : MonoBehaviour {
	private Animator anim;
	bool godBlessed;
	float timeGodBlessed;
	GameObject godLight;
	private Thirang th;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		timeGodBlessed = 0;
		godLight = GameObject.Find ("Goddess' Blessing Light");

		th = GetComponent<Thirang> ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		//Berserk
		if (Input.GetButtonDown ("Berserk")) {
			anim.SetTrigger ("Berserk");
			th.currentAbility = "berserk";
		}

		//Cyclone Spin
		if (Input.GetButtonDown ("Cyclone Spin")) {
			anim.SetTrigger ("Cyclone Spin");
			th.currentAbility = "cycloneSpin";
		}

		//Magic Arrow
		if (Input.GetButtonDown ("Magic Arrow")) {
			anim.SetTrigger ("Magic Arrow");
			th.currentAbility = "magicArrow";
		}

		if (ThirangSaT.abilitiesStates.ContainsValue (stateInfo.fullPathHash)) {
			foreach (KeyValuePair <string, int> key in ThirangSaT.abilitiesStates) {
				if (stateInfo.fullPathHash != key.Value) {
					int index = key.Key.IndexOf ("Back");
					if(index < 0)
						anim.ResetTrigger (key.Key);
					else
						anim.ResetTrigger (key.Key.Remove(index, 4));
				}
			}
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
