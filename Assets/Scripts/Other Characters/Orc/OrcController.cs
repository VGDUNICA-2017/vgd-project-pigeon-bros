using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : EnemyController {

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		enemy = this.gameObject.GetComponent<Orc> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		th = thirang.GetComponent<Thirang> ();

		anim.SetBool ("Run", true);

		UpdatePosition ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		
		if (!enemy.ThirangOnCycloneSpin()) {			//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
			UpdatePosition();
		}

		//Attack
		//Attack-Run state switching control
		if (thirangDistance < distanceThreshold &&
		   stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
		   stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
		{
			anim.SetBool ("Run", false);
			anim.SetTrigger ("Attack");
		} else {
			if (thirangDistance > distanceThreshold) {
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
		if (!deathStart) 
		{
			EnemyDead ();
		}

		DeathAnimation (stateInfo);

	}

	public bool OnFrameToHit() {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash)
			return stateInfo.normalizedTime > 0.3f && stateInfo.normalizedTime < 0.61f;

		return false;
	}
}