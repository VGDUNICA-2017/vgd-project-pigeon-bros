using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverride : MonoBehaviour {
	Animator anim;
	Rigidbody ThirangRb;
	GroundRaycast[] rayManager;
	MovingGround mG;

	bool launched, higherJump;	//higherJump: when it's true Thirang will jump higher to avoid moving up ground bug
	public bool jumpDownEnd { get; set; }
	float jumpTime;
	float heightJump, lengthJump;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		ThirangRb = GetComponent <Rigidbody> ();
		rayManager = GetComponentsInChildren <GroundRaycast> ();

		lengthJump = 5f;
		heightJump = 12f;
	}

	void FixedUpdate () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo nextInfo = anim.GetNextAnimatorStateInfo (0);

		if ((stateInfo.fullPathHash == ThirangSaT.runStateHash && nextInfo.fullPathHash == ThirangSaT.jumpStart) ||
			(stateInfo.fullPathHash == ThirangSaT.runBackStateHash && nextInfo.fullPathHash == ThirangSaT.jumpBackStart)) 
		{
			lengthJump = 3f;
			heightJump = 12f;
			anim.applyRootMotion = false;
		}

		if (((stateInfo.fullPathHash == ThirangSaT.idleStateHash || stateInfo.fullPathHash == ThirangSaT.walkStateHash) &&
		    	nextInfo.fullPathHash == ThirangSaT.jumpStart) ||
		    ((stateInfo.fullPathHash == ThirangSaT.idleBackStateHash || stateInfo.fullPathHash == ThirangSaT.walkBackStateHash) &&
				nextInfo.fullPathHash == ThirangSaT.jumpBackStart)) 
		{
			lengthJump = 5f;
			heightJump = 12f;
			anim.applyRootMotion = false;
		}

		//Fixed a bug that made Thirang's jump really short and low
		if (transform.parent) {
			if (transform.parent.gameObject.CompareTag ("BuggedGround")) {
				if (!mG)	//if it is null it will be assigned, in this way the assignement will be done just one time
					mG = transform.parent.parent.GetComponent<MovingGround> ();

				if (mG.state == MovingGround.State.MovingUp) {
					higherJump = true;
				} else {
					higherJump = false;
				}
			}
		} else {
			mG = null;
			if (!anim.GetBool ("IsJumping")) {
				higherJump = false;
			}
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
			lengthJump = 5f;
			heightJump = 12f;
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

				if (higherJump) {
					lengthJump = 9f;
					heightJump = 14f;
				}

				Vector3 forceDir = new Vector3 (lengthJump, heightJump);
				ThirangRb.AddForce (forceDir, ForceMode.VelocityChange);
				launched = true;
				higherJump = false;
				print (lengthJump + " " + heightJump);
			}
		}

		if (stateInfo.fullPathHash == ThirangSaT.jumpBackStart) {

			if (higherJump) {
				lengthJump = 12f;
				heightJump = 20f;
			}

			anim.applyRootMotion = false;
			if (!launched) {
				Vector3 forceDir = new Vector3 (-lengthJump, heightJump);
				ThirangRb.AddForce (forceDir, ForceMode.VelocityChange);
				launched = true;
				higherJump = false;
			}
		}

	}
}	
