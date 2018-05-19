using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverride : MonoBehaviour {
	Animator anim;
	Rigidbody ThirangRb;
	GroundRaycast[] rayManager;

	bool launched;
	public bool jumpDownEnd { get; set; }
	float jumpTime;
	float heightJump, lengthJump;
	public LayerMask maskLayer;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		ThirangRb = GetComponent <Rigidbody> ();
		rayManager = GetComponentsInChildren <GroundRaycast> ();

		lengthJump = 40f;
		heightJump = 120f;
	}

	void FixedUpdate () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo nextInfo = anim.GetNextAnimatorStateInfo (0);

		if ((stateInfo.fullPathHash == ThirangSaT.runStateHash && nextInfo.fullPathHash == ThirangSaT.jumpStart) ||
			(stateInfo.fullPathHash == ThirangSaT.runBackStateHash && nextInfo.fullPathHash == ThirangSaT.jumpBackStart)) 
		{
			lengthJump = 32f;
			anim.applyRootMotion = false;
		}

		if (((stateInfo.fullPathHash == ThirangSaT.idleStateHash || stateInfo.fullPathHash == ThirangSaT.walkStateHash) &&
		    	nextInfo.fullPathHash == ThirangSaT.jumpStart) ||
		    ((stateInfo.fullPathHash == ThirangSaT.idleBackStateHash || stateInfo.fullPathHash == ThirangSaT.walkBackStateHash) &&
				nextInfo.fullPathHash == ThirangSaT.jumpBackStart)) 
		{
			lengthJump = 40f;
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
			lengthJump = 20f;
			anim.SetBool ("IsJumping", false);
			foreach (GroundRaycast gR in rayManager) {
				gR.Landed ();
			}
			jumpDownEnd = true;
		} else if (jumpDownEnd) {	//Called only once at the end of "JumpDown" animation
			jumpDownEnd = false;
			anim.applyRootMotion = true;
		}

		if (stateInfo.fullPathHash == ThirangSaT.jumpStart) {
			anim.applyRootMotion = false;
			if (!launched) {
				Vector3 forceDir = new Vector3 (lengthJump, heightJump);
				ThirangRb.AddForce (forceDir, ForceMode.Impulse);
				launched = true;
			}
		}

		if (stateInfo.fullPathHash == ThirangSaT.jumpBackStart) {
			anim.applyRootMotion = false;
			if (!launched) {
				Vector3 forceDir = new Vector3 (-lengthJump, heightJump);
				ThirangRb.AddForce (forceDir, ForceMode.Impulse);
				launched = true;
			}
		}

	}
}	
