using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverride : MonoBehaviour {
	Animator anim;
	Rigidbody ThirangRb;
	GroundRaycast[] rayManager;
	StatesAndTransitions SaT;

	bool launched;
	float jumpTime;
	float heightJump, lengthJump;
	public LayerMask maskLayer;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		ThirangRb = GetComponent <Rigidbody> ();
		rayManager = GetComponentsInChildren <GroundRaycast> ();
		SaT = new StatesAndTransitions ();

		lengthJump = 155f;
		heightJump = 6000f;
	}

	void FixedUpdate () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo nextState = anim.GetNextAnimatorStateInfo (0);

		if ((stateInfo.fullPathHash == SaT.runStateHash && nextState.fullPathHash == SaT.jumpStart) ||
			(stateInfo.fullPathHash == SaT.runBackStateHash && nextState.fullPathHash == SaT.jumpBackStart)) 
		{
			lengthJump = 190f;
			anim.applyRootMotion = false;
		}

		//Emergency exit from jumpIdle animation, caused by "collision" error with ground
		if (stateInfo.fullPathHash == SaT.jumpIdle || stateInfo.fullPathHash == SaT.jumpBackIdle) {

			jumpTime += Time.deltaTime;
			if (jumpTime >= 20f) {
				anim.applyRootMotion = true;
				anim.SetTrigger ("JumpDown");
			}
		} else {
			jumpTime = 0f;
		}

		if (stateInfo.fullPathHash == SaT.jumpDown || stateInfo.fullPathHash == SaT.jumpBackDown) {
			launched = false;
			anim.ResetTrigger ("JumpDown");
			lengthJump = 155f;
			anim.SetBool ("IsJumping", false);
			foreach (GroundRaycast gR in rayManager) {
				gR.Landing ();
			}
		}

		if (stateInfo.fullPathHash == SaT.jumpStart) {
			anim.applyRootMotion = false;
			if (!launched) {
				Vector3 forceDir = new Vector3 (lengthJump, heightJump);
				ThirangRb.AddForce (forceDir);
				launched = true;
			}
		}

		if (stateInfo.fullPathHash == SaT.jumpBackStart) {
			anim.applyRootMotion = false;
			if (!launched) {
				Vector3 forceDir = new Vector3 (-lengthJump, heightJump);
				ThirangRb.AddForce (forceDir);
				launched = true;
			}
		}
	}
}	
