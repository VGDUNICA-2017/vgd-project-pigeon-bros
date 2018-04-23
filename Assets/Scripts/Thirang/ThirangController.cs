using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirangController : MonoBehaviour {
	private Animator anim;
	public GameObject LeftFoot, RightFoot;
	GroundRaycast rayManagerLeft, rayManagerRight;
	StatesAndTransitions SaT;
	private Thirang th;

	float timerSpecIdle;

	bool fightStarted;
	bool blockStarted;

	float speedAdjRot = 0.24f;
	float speedBackAdjRot = 0.17f;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		rayManagerLeft = LeftFoot.GetComponent <GroundRaycast> ();
		rayManagerRight = RightFoot.GetComponent <GroundRaycast> ();

		SaT = new StatesAndTransitions ();
		th = GetComponent <Thirang> ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo blockInfo = anim.GetCurrentAnimatorStateInfo (1);
		AnimatorStateInfo specIdleInfo = anim.GetCurrentAnimatorStateInfo (2);

		AnimatorTransitionInfo transInfo = anim.GetAnimatorTransitionInfo (0);

		if (rayManagerLeft.onGround || rayManagerRight.onGround) {
			anim.applyRootMotion = false;
		} else {
			if (stateInfo.fullPathHash != SaT.jumpStart && stateInfo.fullPathHash != SaT.jumpIdle && stateInfo.fullPathHash != SaT.jumpDown &&
				stateInfo.fullPathHash != SaT.jumpBackStart && stateInfo.fullPathHash != SaT.jumpBackIdle && stateInfo.fullPathHash != SaT.jumpBackDown) 
			{
				anim.applyRootMotion = true;
			}
		}

		//lock z Position
		if (!anim.GetBool ("IsFighting"))
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0);

		if ((stateInfo.fullPathHash == SaT.idleStateHash || stateInfo.fullPathHash == SaT.backIdleStateHash)
		    && (specIdleInfo.fullPathHash == SaT.specIdle || specIdleInfo.fullPathHash == SaT.specIdleBack)
		    && !anim.GetBool ("IsShielding")) {
			timerSpecIdle += Time.deltaTime;
			anim.SetFloat ("TimerSpecIdle", timerSpecIdle);
		} else {
			timerSpecIdle = 0;
			anim.SetFloat ("TimerSpecIdle", timerSpecIdle);
		}
		if (timerSpecIdle >= 10f) {
			anim.SetInteger ("RandomIdle", Random.Range (1, 4));
		}
		
		if (transInfo.fullPathHash == SaT.idleTransIdleBack) {
			anim.SetBool ("IsFacingRight", false);
			timerSpecIdle = 0;
		}

		if (transInfo.fullPathHash == SaT.idleBackTransIdle) {
			anim.SetBool ("IsFacingRight", true);
			timerSpecIdle = 0;
		}

		if (stateInfo.fullPathHash == SaT.walkStateHash || stateInfo.fullPathHash == SaT.runStateHash ||
		    stateInfo.fullPathHash == SaT.walkBackStateHash || stateInfo.fullPathHash == SaT.runBackStateHash ||
		    stateInfo.fullPathHash == SaT.idleStateHash || stateInfo.fullPathHash == SaT.backIdleStateHash) {
			Vector3 rot = transform.eulerAngles;
			rot.y = 90f;
			transform.eulerAngles = rot;
		}
	
		float speed = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", speed);

		//Run Input Control
		if (Input.GetButton ("Run")) {
			anim.SetBool ("Run", true);
		} else {
			anim.SetBool ("Run", false);
		}

		//Block Input Control
		if (Input.GetButtonDown ("Shield")) {
			blockStarted = true;
		}

		if (Input.GetButton ("Shield") && blockStarted && !anim.GetBool ("IsFighting") &&
		    !anim.GetBool ("Run") && !anim.GetBool ("IsJumping")) {
			anim.SetBool ("IsShielding", true);
		} else if (blockInfo.fullPathHash == SaT.blockStateHash || blockInfo.fullPathHash == SaT.blockBackStateHash) {
			blockStarted = false;
		}
		
		if (blockStarted || blockInfo.fullPathHash == SaT.blockStateHash || blockInfo.fullPathHash == SaT.blockBackStateHash
		    || Input.GetButton ("Shield")) {
			if (blockInfo.fullPathHash == SaT.blockStateHash || blockInfo.fullPathHash == SaT.blockBackStateHash) {
				blockStarted = false;
			}
		} else {
			anim.SetBool ("IsShielding", false);
		}

		//Adjusting the override of the animator block layer
		if (blockInfo.fullPathHash == SaT.blockIdleStateHash || blockInfo.fullPathHash == SaT.blockIdleBackStateHash) {
			Vector3 newPos = transform.position;
			newPos.x += speed * Time.deltaTime / 2;
			transform.position = newPos;
		}

		//Fight Input Control
		if (fightStarted ||
		    stateInfo.fullPathHash == SaT.slash1StateHash || stateInfo.fullPathHash == SaT.slash2StateHash ||
		    stateInfo.fullPathHash == SaT.slash1BackStateHash || stateInfo.fullPathHash == SaT.slash2BackStateHash) {
			if (stateInfo.fullPathHash == SaT.slash1StateHash || stateInfo.fullPathHash == SaT.slash2StateHash ||
			    stateInfo.fullPathHash == SaT.slash1BackStateHash || stateInfo.fullPathHash == SaT.slash2BackStateHash) {
				fightStarted = false;
			}
			anim.SetBool ("IsFighting", true);

		} else {
			anim.SetBool ("IsFighting", false);
		}
		
		if (Input.GetButtonDown ("Slash") && !anim.GetBool ("IsShielding") &&
		    (stateInfo.fullPathHash == SaT.idleStateHash || stateInfo.fullPathHash == SaT.backIdleStateHash ||
		    stateInfo.fullPathHash == SaT.walkStateHash || stateInfo.fullPathHash == SaT.runStateHash ||
		    stateInfo.fullPathHash == SaT.walkBackStateHash || stateInfo.fullPathHash == SaT.runBackStateHash)) {
			anim.SetTrigger ("Slash 1");
			anim.SetBool ("IsFighting", true);
			fightStarted = true;
			th.currentAbility = "autoAttack";
		}
		if (Input.GetButtonDown ("Slash") &&
		    (stateInfo.fullPathHash == SaT.slash1StateHash || stateInfo.fullPathHash == SaT.slash1BackStateHash)) {
			anim.SetTrigger ("Slash 2");
			th.currentAbility = "autoAttack";
		}

		//Adjusting slash animation rotation
		if (stateInfo.fullPathHash == SaT.slash1StateHash || stateInfo.fullPathHash == SaT.slash1BackStateHash) {
			Vector3 adjustRot = transform.eulerAngles;
			adjustRot.y += speedAdjRot;
			transform.eulerAngles = adjustRot;
			transform.position = new Vector3 (transform.position.x, transform.position.y); //Lock z position
		}

		if (stateInfo.fullPathHash == SaT.slash2StateHash || stateInfo.fullPathHash == SaT.slash2BackStateHash) {
			Vector3 adjustRot = transform.eulerAngles;
			adjustRot.y -= speedBackAdjRot;
			transform.eulerAngles = adjustRot;
		}

		//Jump
		if (Input.GetButtonDown ("Jump") && !anim.GetBool ("IsShielding") && !anim.GetBool ("IsFighting") &&
		    stateInfo.fullPathHash != SaT.jumpStart && stateInfo.fullPathHash != SaT.jumpIdle && stateInfo.fullPathHash != SaT.jumpDown &&
		    stateInfo.fullPathHash != SaT.jumpBackStart && stateInfo.fullPathHash != SaT.jumpBackIdle && stateInfo.fullPathHash != SaT.jumpBackDown) {
			anim.SetTrigger ("Jump");
			anim.SetBool ("IsJumping", true);
		}

		if (stateInfo.fullPathHash == SaT.jumpStart || stateInfo.fullPathHash == SaT.jumpIdle || stateInfo.fullPathHash == SaT.jumpDown ||
		    stateInfo.fullPathHash == SaT.jumpBackStart || stateInfo.fullPathHash == SaT.jumpBackIdle || stateInfo.fullPathHash == SaT.jumpBackDown) {
			anim.ResetTrigger ("Jump");
		}
	}
}
