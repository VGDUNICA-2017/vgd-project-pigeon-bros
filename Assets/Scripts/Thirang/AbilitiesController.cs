using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesController : MonoBehaviour {
	private Animator anim;
	private Thirang th;
	private GroundRaycast rayManagerLeft, rayManagerRight;

	public bool onCycloneSpin { get; set; }

	public GameObject magicArrow;
	bool arrowSpawned;

	bool godBlessed;
	float timeGodBlessed;
	Light godLight;

	int gameLevel;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		timeGodBlessed = 0;
		godLight = GetComponentInChildren<Light> ();

		th = GetComponent<Thirang> ();

		ThirangController thCtrl = GetComponent<ThirangController> ();
		rayManagerLeft = thCtrl.LeftFoot.GetComponent<GroundRaycast> ();
		rayManagerRight = thCtrl.RightFoot.GetComponent<GroundRaycast> ();

		if (!PlayerPrefs.HasKey ("gameLevel")) {
			PlayerPrefs.SetInt ("gameLevel", 1);
		}

		gameLevel = PlayerPrefs.GetInt ("gameLevel", 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (!th.isDead) {

			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

			//Berserk
			/*Being child of MovingGround makes berserk animation buggy because of scale animation*/
			if (!rayManagerLeft.onMovingGround && !rayManagerRight.onMovingGround) {
				if (Input.GetButtonDown ("Berserk")) {
					if (stateInfo.fullPathHash != ThirangSaT.abilitiesStates ["Berserk"] ||
					   stateInfo.fullPathHash != ThirangSaT.abilitiesStates ["BerserkBack"]) {
						anim.SetTrigger ("Berserk");
						th.SetCurrentAbility ("berserk");
						th.Berserk ();
					}
				}
			}

			if (gameLevel > 1) {

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

				if (gameLevel > 2) {

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

					if (gameLevel > 3) {

						//Goddess' Blessing
						if (Input.GetButtonDown ("Goddess' Blessing")) {
							godLight.enabled = true;
							godBlessed = true;
							th.SetCurrentAbility ("goddessBlessing");
						}

						if (godBlessed) {
							timeGodBlessed += Time.deltaTime;
							if (timeGodBlessed >= th.timeGodBlessed) {
								godLight.GetComponent<Light> ().enabled = false;
								godBlessed = false;
								timeGodBlessed = 0;
								th.abilityState = new Character.Ability (0, Character.DamageType.none);
							}
						}
					}
				}
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

		//Instantiates new Arrow and sets the direction where it must travel
		Instantiate (magicArrow, arrowPos, Quaternion.Euler(th.transform.eulerAngles * direction)).GetComponent<MagicArrow>().Direction(direction);
	}
}
