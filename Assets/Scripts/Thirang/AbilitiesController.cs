using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesController : MonoBehaviour {
	private Animator anim;
	private Thirang th;

	public bool onCycloneSpin { get; set; }

	public GameObject magicArrow;
	bool arrowSpawned;

	bool godBlessed;
	float timeGodBlessed;
	Light godLight;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		timeGodBlessed = 0;
		godLight = GetComponentInChildren<Light> ();

		th = GetComponent<Thirang> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!th.isDead) {

			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

			//Berserk
			if (Input.GetButtonDown ("Berserk")) {
				if (stateInfo.fullPathHash != ThirangSaT.abilitiesStates ["Berserk"] &&
				    stateInfo.fullPathHash != ThirangSaT.abilitiesStates ["BerserkBack"]) {
					anim.SetTrigger ("Berserk");
					th.SetCurrentAbility ("berserk");
					th.Berserk ();
				}
			}

			//Cyclone Spin
			if (Input.GetButtonDown ("Cyclone Spin")) {
				anim.SetTrigger ("Cyclone Spin");
				th.SetCurrentAbility ("cycloneSpin");
			}

			if (stateInfo.fullPathHash == ThirangSaT.abilitiesStates ["Cyclone Spin"] ||
			    stateInfo.fullPathHash == ThirangSaT.abilitiesStates ["Cyclone SpinBack"]) {
				anim.SetBool ("IsFighting", true);
				onCycloneSpin = true;
			} else if (onCycloneSpin) {
				anim.SetBool ("IsFighting", false);
				onCycloneSpin = false;
			}

			//Magic Arrow
			if (Input.GetButtonDown ("Magic Arrow")) {
				anim.SetTrigger ("Magic Arrow");
				th.SetCurrentAbility ("magicArrow");
			}

			if (stateInfo.fullPathHash == ThirangSaT.abilitiesStates ["Magic Arrow"] ||
			    stateInfo.fullPathHash == ThirangSaT.abilitiesStates ["Magic ArrowBack"]) {
				if (stateInfo.normalizedTime >= 0.56f && !arrowSpawned) {
					SpawnMagicArrow ();
					arrowSpawned = true;
				}	
			} else {
				arrowSpawned = false;
			}

			if (ThirangSaT.abilitiesStates.ContainsValue (stateInfo.fullPathHash)) {
				foreach (KeyValuePair <string, int> key in ThirangSaT.abilitiesStates) {
					if (stateInfo.fullPathHash != key.Value) {
						int index = key.Key.IndexOf ("Back");
						if (index < 0)
							anim.ResetTrigger (key.Key);
						else
							anim.ResetTrigger (key.Key.Remove (index, 4));
					}
				}
			}

			//Goddess' Blessing
			if (Input.GetButtonDown ("Goddess' Blessing")) {
				godLight.enabled = true;
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

	void SpawnMagicArrow() {
		Vector3 arrowPos = th.transform.position;
		int direction;

		if (anim.GetBool ("IsFacingRight")) {
			direction = 1;
		} else {
			direction = -1;

		}

		arrowPos.y += 3f;
		arrowPos.x += 3.8f * direction;

		//Instantiate new Arrow and set who is the caster of it (only thirang; it useful to control damage dealt by the arrow)
		Instantiate (magicArrow, arrowPos, Quaternion.Euler(th.transform.eulerAngles * direction)).GetComponent<MagicArrow>().Direction(direction);
	}
}
