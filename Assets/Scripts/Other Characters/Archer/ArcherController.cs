using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour {
	Animator anim;
	GameObject thirang;
	Thirang th;
	Archer archer;
	public GameObject arrow;

	public float distArcherPlayer;

	bool deathStart;
	bool arrowSpawned;

	public bool isFacingLeft { get; set; }

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		archer = this.gameObject.GetComponent<Archer> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		th = thirang.GetComponent<Thirang> ();

		anim.SetBool ("Run", true);

		float dist = transform.position.x - thirang.transform.position.x;

		if (dist >= 0) {
			anim.SetBool ("IsFacingLeft", true);
			isFacingLeft = true;
		}
		if (dist < 0) {
			anim.SetBool ("IsFacingLeft", false);
			isFacingLeft = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		float dist = transform.position.x - thirang.transform.position.x;
		float offset = Mathf.Abs(dist);

		//Adjust Archer position and rotation
		transform.position = new Vector3 (transform.position.x, transform.position.y);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);

		if (!archer.ThirangOnCycloneSpin()) {			//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
			if (dist >= 0) {
				anim.SetBool ("IsFacingLeft", true);
				isFacingLeft = true;
			}
			if (dist < 0) {
				anim.SetBool ("IsFacingLeft", false);
				isFacingLeft = false;
			}
		}

		//Attack
		//Attack-Run state switching control
		if (!th.isDead) {
			if (offset < distArcherPlayer &&
			    stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
			    stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
			{
				anim.SetBool ("Run", false);
				anim.SetTrigger ("Attack");
			} else {
				if (offset > distArcherPlayer) {
					anim.SetBool ("Run", true);
					anim.ResetTrigger ("Attack");
				}
			}
		}

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
			if (stateInfo.normalizedTime > 0.85f && !arrowSpawned) {
				arrow.SetActive (false);
				SpawnArrow ();
				arrowSpawned = true;
			}

			if (stateInfo.normalizedTime > 0.32f && !arrowSpawned)
				arrow.SetActive (true);
			
		} else {
			arrowSpawned = false;
		}

		//Death
		if (archer.health <= 0 && !deathStart) 
		{
			anim.SetTrigger ("Death");
			deathStart = true;
			archer.OnDeath ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				archer.fadingDeath = true;
			}
		}

	}

	void SpawnArrow() {
		Vector3 newRot;
		int direction;

		if (anim.GetBool ("IsFacingLeft")) {
			direction = -1;
			newRot = new Vector3 (0, 270);
		} else {
			direction = 1;
			newRot = new Vector3 (0, 90);
		}

		//Instantiates new Arrow and sets the direction where it must travel
		OnArrowHit newArrow = Instantiate (arrow, arrow.transform.position, Quaternion.Euler (newRot), null).AddComponent<OnArrowHit> ();
		newArrow.gameObject.tag = "Destroyable";
		newArrow.gameObject.transform.localScale = new Vector3 (5f, 5f, 2.5f);
		newArrow.gameObject.SetActive (true);
		newArrow.archer = archer;
		newArrow.Direction (direction);
	}
}
