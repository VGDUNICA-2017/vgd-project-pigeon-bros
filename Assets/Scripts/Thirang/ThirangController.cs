using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirangController : MonoBehaviour {
	private Animator anim;
	public GameObject LeftFoot, RightFoot;
	GroundRaycast rayManagerLeft, rayManagerRight;
	private Thirang th;

	float timerSpecIdle;

	bool fightStarted;
	bool blockStarted;

	float speedAdjRot = 0.24f;
	float speedBackAdjRot = 0.17f;
	bool slash2Hit;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		rayManagerLeft = LeftFoot.GetComponent <GroundRaycast> ();
		rayManagerRight = RightFoot.GetComponent <GroundRaycast> ();

		th = GetComponent <Thirang> ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo blockInfo = anim.GetCurrentAnimatorStateInfo (1);
		AnimatorStateInfo specIdleInfo = anim.GetCurrentAnimatorStateInfo (2);

		AnimatorTransitionInfo transInfo = anim.GetAnimatorTransitionInfo (0);

		if (rayManagerLeft.onGround || rayManagerRight.onGround) {
			anim.applyRootMotion = true;
		} else {
			if (stateInfo.fullPathHash != ThirangSaT.jumpStart && stateInfo.fullPathHash != ThirangSaT.jumpIdle && 
				stateInfo.fullPathHash != ThirangSaT.jumpDown && stateInfo.fullPathHash != ThirangSaT.jumpBackStart && 
				stateInfo.fullPathHash != ThirangSaT.jumpBackIdle && stateInfo.fullPathHash != ThirangSaT.jumpBackDown) 
			{
				anim.applyRootMotion = false;
			}
		}

		//lock z Position
		if (!anim.GetBool ("IsFighting"))
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0);

		if ((stateInfo.fullPathHash == ThirangSaT.idleStateHash || stateInfo.fullPathHash == ThirangSaT.idleBackStateHash)
			&& (specIdleInfo.fullPathHash == ThirangSaT.specIdle || specIdleInfo.fullPathHash == ThirangSaT.specIdleBack)
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
		
		if (transInfo.fullPathHash == ThirangSaT.idleTransIdleBack) {
			anim.SetBool ("IsFacingRight", false);
			timerSpecIdle = 0;
		}

		if (transInfo.fullPathHash == ThirangSaT.idleBackTransIdle) {
			anim.SetBool ("IsFacingRight", true);
			timerSpecIdle = 0;
		}

		if (stateInfo.fullPathHash == ThirangSaT.walkStateHash || stateInfo.fullPathHash == ThirangSaT.runStateHash ||
			stateInfo.fullPathHash == ThirangSaT.walkBackStateHash || stateInfo.fullPathHash == ThirangSaT.runBackStateHash ||
			stateInfo.fullPathHash == ThirangSaT.idleStateHash || stateInfo.fullPathHash == ThirangSaT.idleBackStateHash) {
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
		} else if (blockInfo.fullPathHash == ThirangSaT.blockStateHash || blockInfo.fullPathHash == ThirangSaT.blockBackStateHash) {
			blockStarted = false;
		}
		
		if (blockStarted || blockInfo.fullPathHash == ThirangSaT.blockStateHash || blockInfo.fullPathHash == ThirangSaT.blockBackStateHash
		    || Input.GetButton ("Shield")) {
			if (blockInfo.fullPathHash == ThirangSaT.blockStateHash || blockInfo.fullPathHash == ThirangSaT.blockBackStateHash) {
				blockStarted = false;
			}
		} else {
			anim.SetBool ("IsShielding", false);
		}

		//Adjusting the override of the animator block layer
		if (blockInfo.fullPathHash == ThirangSaT.blockIdleStateHash || blockInfo.fullPathHash == ThirangSaT.blockIdleBackStateHash) {
			Vector3 newPos = transform.position;
			newPos.x += speed * Time.deltaTime / 2;
			transform.position = newPos;
		}

		//Fight Input Control
		if (fightStarted ||
			stateInfo.fullPathHash == ThirangSaT.slash1StateHash || stateInfo.fullPathHash == ThirangSaT.slash2StateHash ||
			stateInfo.fullPathHash == ThirangSaT.slash1BackStateHash || stateInfo.fullPathHash == ThirangSaT.slash2BackStateHash) 
		{
			if (stateInfo.fullPathHash == ThirangSaT.slash1StateHash || stateInfo.fullPathHash == ThirangSaT.slash1BackStateHash) {
				fightStarted = false;
				anim.ResetTrigger ("Slash 1");
			}
			if (stateInfo.fullPathHash == ThirangSaT.slash2StateHash || stateInfo.fullPathHash == ThirangSaT.slash2BackStateHash) {
				fightStarted = false;
				anim.ResetTrigger ("Slash 2");
			}
			anim.SetBool ("IsFighting", true);
		} else {
			anim.SetBool ("IsFighting", false);
			slash2Hit = false;
		}
		
		if (Input.GetButtonDown ("Slash") && !anim.GetBool ("IsShielding") &&
			(stateInfo.fullPathHash == ThirangSaT.idleStateHash || stateInfo.fullPathHash == ThirangSaT.idleBackStateHash ||
				stateInfo.fullPathHash == ThirangSaT.walkStateHash || stateInfo.fullPathHash == ThirangSaT.runStateHash ||
				stateInfo.fullPathHash == ThirangSaT.walkBackStateHash || stateInfo.fullPathHash == ThirangSaT.runBackStateHash)) 
		{
			anim.SetTrigger ("Slash 1");
			anim.SetBool ("IsFighting", true);
			fightStarted = true;

			th.newAttack = true;
			th.currentAbility = "autoAttack";
		}
		if (Input.GetButtonDown ("Slash") &&
			(stateInfo.fullPathHash == ThirangSaT.slash1StateHash || stateInfo.fullPathHash == ThirangSaT.slash1BackStateHash)) {
			anim.SetTrigger ("Slash 2");

			th.currentAbility = "autoAttack";
		}

		if (stateInfo.fullPathHash == ThirangSaT.slash2StateHash || stateInfo.fullPathHash == ThirangSaT.slash2BackStateHash) {
			if (stateInfo.normalizedTime > 0.38f && !slash2Hit) {
				th.newAttack = true;
				slash2Hit = true;
			}
		}

		//Adjusting slash animation rotation
		if (stateInfo.fullPathHash == ThirangSaT.slash1StateHash || stateInfo.fullPathHash == ThirangSaT.slash1BackStateHash) {
			Vector3 adjustRot = transform.eulerAngles;
			adjustRot.y += speedAdjRot;
			transform.eulerAngles = adjustRot;
			transform.position = new Vector3 (transform.position.x, transform.position.y); //Lock z position
		}

		if (stateInfo.fullPathHash == ThirangSaT.slash2StateHash || stateInfo.fullPathHash == ThirangSaT.slash2BackStateHash) {
			Vector3 adjustRot = transform.eulerAngles;
			adjustRot.y -= speedBackAdjRot;
			transform.eulerAngles = adjustRot;
		}

		//Jump
		if (Input.GetButtonDown ("Jump") && !anim.GetBool ("IsShielding") && !anim.GetBool ("IsFighting") &&
			stateInfo.fullPathHash != ThirangSaT.jumpStart && stateInfo.fullPathHash != ThirangSaT.jumpIdle && 
			stateInfo.fullPathHash != ThirangSaT.jumpDown && stateInfo.fullPathHash != ThirangSaT.jumpBackStart && 
			stateInfo.fullPathHash != ThirangSaT.jumpBackIdle && stateInfo.fullPathHash != ThirangSaT.jumpBackDown) 
		{
			anim.SetTrigger ("Jump");
			anim.SetBool ("IsJumping", true);
		}

		if (stateInfo.fullPathHash == ThirangSaT.jumpStart || stateInfo.fullPathHash == ThirangSaT.jumpIdle || 
			stateInfo.fullPathHash == ThirangSaT.jumpDown || stateInfo.fullPathHash == ThirangSaT.jumpBackStart || 
			stateInfo.fullPathHash == ThirangSaT.jumpBackIdle || stateInfo.fullPathHash == ThirangSaT.jumpBackDown) 
		{
			anim.ResetTrigger ("Jump");
		}
	}
}
