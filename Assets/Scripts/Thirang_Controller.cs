using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirang_Controller : MonoBehaviour {
	private Animator anim;

	//Base Layer States
	protected readonly int idleStateHash = Animator.StringToHash ("Base Layer.Idle");
	protected readonly int backIdleStateHash = Animator.StringToHash ("Base Layer.Back.Idle Back");
	protected readonly int walkStateHash = Animator.StringToHash ("Base Layer.Walk");
	protected readonly int walkBackStateHash = Animator.StringToHash ("Base Layer.Back.Walk Back");
	protected readonly int runStateHash = Animator.StringToHash ("Base Layer.Run");
	protected readonly int runBackStateHash = Animator.StringToHash ("Base Layer.Back.Run Back");

	//Idle Transitions
	protected readonly int idleTransIdleBack = Animator.StringToHash ("Base Layer.Idle -> Base Layer.Back.Idle Back");
	protected readonly int idleBackTransIdle = Animator.StringToHash ("Base Layer.Back.Idle Back -> Base Layer.Idle");

	//Fight States
	protected readonly int slash1StateHash = Animator.StringToHash ("Base Layer.Fight.Slash 1");
	protected readonly int slash2StateHash = Animator.StringToHash ("Base Layer.Fight.Slash 2");
	protected readonly int slash1BackStateHash = Animator.StringToHash ("Base Layer.Back.Fight Back.Slash 1 Back");
	protected readonly int slash2BackStateHash = Animator.StringToHash ("Base Layer.Back.Fight Back.Slash 2 Back");

	//Block States
	protected readonly int blockStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block");
	protected readonly int blockBackStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block Back");

	//Jump States
	protected readonly int jumpState = Animator.StringToHash ("Base Layer.Jump.Jump Start");
	protected readonly int jumpBackState = Animator.StringToHash ("Base Layer.Back.Jump Back");

	//Special Idles States
	float timerSpecIdle;
	protected readonly int specIdle = Animator.StringToHash ("Special Idles.Idle");
	protected readonly int specIdleBack = Animator.StringToHash ("Special Idles.Idle Back");

	bool fightStarted;
	bool blockStarted;

	float speedAdjRot = 0.24f;
	float speedBackAdjRot = 0.17f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo blockInfo = anim.GetCurrentAnimatorStateInfo (1);
		AnimatorStateInfo specIdleInfo = anim.GetCurrentAnimatorStateInfo (2);

		AnimatorTransitionInfo transInfo = anim.GetAnimatorTransitionInfo (0);

		if ((stateInfo.fullPathHash == idleStateHash || stateInfo.fullPathHash == backIdleStateHash)
		    && (specIdleInfo.fullPathHash == specIdle || specIdleInfo.fullPathHash == specIdleBack)
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
			
		if (transInfo.fullPathHash == idleTransIdleBack) {
			anim.SetBool ("IsFacingRight", false);
			timerSpecIdle = 0;
		}

		if (transInfo.fullPathHash == idleBackTransIdle) {
			anim.SetBool ("IsFacingRight", true);
			timerSpecIdle = 0;
		}

		if (stateInfo.fullPathHash == walkStateHash || stateInfo.fullPathHash == runStateHash ||
		    stateInfo.fullPathHash == walkBackStateHash || stateInfo.fullPathHash == runBackStateHash ||
		    stateInfo.fullPathHash == idleStateHash || stateInfo.fullPathHash == backIdleStateHash) {
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

		if (Input.GetButton ("Shield") && blockStarted && !anim.GetBool ("IsFighting") && !anim.GetBool ("Run")) {
			anim.SetBool ("IsShielding", true);
		} else if (blockInfo.fullPathHash == blockStateHash || blockInfo.fullPathHash == blockBackStateHash) {
			blockStarted = false;
		}
			
		if (blockStarted || blockInfo.fullPathHash == blockStateHash || blockInfo.fullPathHash == blockBackStateHash
		    || Input.GetButton ("Shield")) {
			if (blockInfo.fullPathHash == blockStateHash || blockInfo.fullPathHash == blockBackStateHash) {
				blockStarted = false;
			}
		} else {
			anim.SetBool ("IsShielding", false);
		}

		//Fight Input Control
		if (fightStarted ||
		    stateInfo.fullPathHash == slash1StateHash || stateInfo.fullPathHash == slash2StateHash ||
		    stateInfo.fullPathHash == slash1BackStateHash || stateInfo.fullPathHash == slash2BackStateHash) {
			if (stateInfo.fullPathHash == slash1StateHash || stateInfo.fullPathHash == slash2StateHash ||
			    stateInfo.fullPathHash == slash1BackStateHash || stateInfo.fullPathHash == slash2BackStateHash) {
				fightStarted = false;
			}
			anim.SetBool ("IsFighting", true);

		} else {
			anim.SetBool ("IsFighting", false);
		}
			
		if (Input.GetButtonDown ("Slash") && !anim.GetBool ("IsShielding") &&
		    (stateInfo.fullPathHash == idleStateHash || stateInfo.fullPathHash == backIdleStateHash ||
				stateInfo.fullPathHash == walkStateHash || stateInfo.fullPathHash == runStateHash ||
				stateInfo.fullPathHash == walkBackStateHash || stateInfo.fullPathHash == runBackStateHash)) {
			anim.SetTrigger ("Slash 1");
			anim.SetBool ("IsFighting", true);
			fightStarted = true;
		}
		if (Input.GetButtonDown ("Slash") &&
		    (stateInfo.fullPathHash == slash1StateHash || stateInfo.fullPathHash == slash1BackStateHash)) {
			anim.SetTrigger ("Slash 2");
		}
	
		if (stateInfo.fullPathHash == slash1StateHash || stateInfo.fullPathHash == slash1BackStateHash) {
			Vector3 adjustRot = transform.eulerAngles;
			adjustRot.y += speedAdjRot;
			transform.eulerAngles = adjustRot;
		}

		if (stateInfo.fullPathHash == slash2StateHash || stateInfo.fullPathHash == slash2BackStateHash) {
			Vector3 adjustRot = transform.eulerAngles;
			adjustRot.y -= speedBackAdjRot;
			transform.eulerAngles = adjustRot;
		}

		//Jump
		if (Input.GetButtonDown ("Jump") && !anim.GetBool ("IsShielding") && !anim.GetBool ("IsFighting") &&
			stateInfo.fullPathHash != jumpState && stateInfo.fullPathHash != jumpBackState)
		{
			anim.SetTrigger ("Jump");
		}

		if (stateInfo.fullPathHash == jumpState || stateInfo.fullPathHash == jumpBackState) {
			anim.ResetTrigger ("Jump");
		}
	}
}
