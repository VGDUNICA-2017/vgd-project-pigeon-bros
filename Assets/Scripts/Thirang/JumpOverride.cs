using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverride : MonoBehaviour {
	Animator anim;
	Rigidbody ThirangRb;
	GroundRaycast[] rayManager;

	bool launched;
	float jumpTime;
	float heightJump, lengthJump;
	public LayerMask maskLayer;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		ThirangRb = GetComponent <Rigidbody> ();
		rayManager = GetComponentsInChildren <GroundRaycast> ();

		lengthJump = 155f;
		heightJump = 6000f;
	}

	void FixedUpdate () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo nextState = anim.GetNextAnimatorStateInfo (0);

		if ((stateInfo.fullPathHash == ThirangSaT.runStateHash && nextState.fullPathHash == ThirangSaT.jumpStart) ||
			(stateInfo.fullPathHash == ThirangSaT.runBackStateHash && nextState.fullPathHash == ThirangSaT.jumpBackStart)) 
		{
			lengthJump = 190f;
			anim.applyRootMotion = false;
		}

		//Emergency exit from jumpIdle animation, caused by "collision" error with ground
		if (stateInfo.fullPathHash == ThirangSaT.jumpIdle || stateInfo.fullPathHash == ThirangSaT.jumpBackIdle) {

			jumpTime += Time.deltaTime;
			if (jumpTime >= 20f) {
				anim.applyRootMotion = true;
				anim.SetTrigger ("JumpDown");
			}
		} else {
			jumpTime = 0f;
		}

		if (stateInfo.fullPathHash == ThirangSaT.jumpDown || stateInfo.fullPathHash == ThirangSaT.jumpBackDown) {
			launched = false;
			anim.ResetTrigger ("JumpDown");
			lengthJump = 155f;
			anim.SetBool ("IsJumping", false);
			foreach (GroundRaycast gR in rayManager) {
				gR.Landing ();
			}
		}

		if (stateInfo.fullPathHash == ThirangSaT.jumpStart) {
			anim.applyRootMotion = false;
			if (!launched) {
				Vector3 forceDir = new Vector3 (lengthJump, heightJump);
				ThirangRb.AddForce (forceDir);
				launched = true;
			}
		}

		if (stateInfo.fullPathHash == ThirangSaT.jumpBackStart) {
			anim.applyRootMotion = false;
			if (!launched) {
				Vector3 forceDir = new Vector3 (-lengthJump, heightJump);
				ThirangRb.AddForce (forceDir);
				launched = true;
			}
		}
	}
}	
