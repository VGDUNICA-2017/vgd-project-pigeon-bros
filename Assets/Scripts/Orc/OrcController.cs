using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour {
	Animator anim;
	GameObject thirang;
	Orc orc;

	public float distOrcPlayer;

	public bool isAttacking { get; set; }

	bool deathStart;

	public bool isFacingLeft { get; set; }

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		orc = this.gameObject.GetComponent<Orc> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
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

		//Adjust Orc position and rotation
		transform.position = new Vector3 (transform.position.x, transform.position.y);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);
		
		if (!orc.ThirangOnCycloneSpin()) {			//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
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
		if (offset < distOrcPlayer &&
			stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
			stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
		{
			anim.SetBool ("Run", false);
			anim.SetTrigger ("Attack");
		} else {
			if (offset > distOrcPlayer) {
				anim.SetBool ("Run", true);
				anim.ResetTrigger ("Attack");
			}
		}

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
			isAttacking = true;
		} else {
			isAttacking = false;
		}

		//Death
		if (orc.health <= 0 && !deathStart) 
		{
			anim.SetTrigger ("Death");
			deathStart = true;
			orc.OnDeath ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				orc.fadingDeath = true;
			}
		}

	}
}