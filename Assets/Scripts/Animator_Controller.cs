using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Controller : MonoBehaviour {
	Animator anim;
	readonly int idleStateHash = Animator.StringToHash ("Base Layer.Idle");
	readonly int backIdleStateHash = Animator.StringToHash ("Base Layer.Walk And Run Back.Idle Back");
	readonly int walkStateHash = Animator.StringToHash ("Base Layer.Sword And Shield Walk");
	readonly int walkBackStateHash = Animator.StringToHash ("Base Layer.Walk And Run Back.Sword And Shield Walk Back");
	readonly int runStateHash = Animator.StringToHash ("Base Layer.Sword And Shield Run");
	readonly int runBackStateHash = Animator.StringToHash ("Base Layer.Walk And Run Back.Sword And Shield Run Back");

	readonly int idleTransIdleBack = Animator.StringToHash ("Base Layer.Idle -> Base Layer.Walk And Run Back.Idle Back");
	readonly int idleBackTransIdle = Animator.StringToHash ("Base Layer.Walk And Run Back.Idle Back -> Base Layer.Idle");

	readonly int readyToFightStateHash = Animator.StringToHash ("Fight.Ready To Fight");
	float timerSpecIdle;

	readonly int specialIdle = Animator.StringToHash ("Special Idles.Idle");
	readonly int specialBackIdle = Animator.StringToHash ("Special Idles.Idle Back");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo fightInfo = anim.GetCurrentAnimatorStateInfo (1);
		AnimatorStateInfo specIdleInfo = anim.GetCurrentAnimatorStateInfo (2);

		if ((stateInfo.fullPathHash == idleStateHash && fightInfo.fullPathHash == readyToFightStateHash
			 && specIdleInfo.fullPathHash == specialIdle)
			|| (stateInfo.fullPathHash == backIdleStateHash && fightInfo.fullPathHash == readyToFightStateHash
			 && specIdleInfo.fullPathHash == specialBackIdle)) 
		{
			timerSpecIdle += Time.deltaTime;
			anim.SetFloat ("TimerSpecIdle", timerSpecIdle);
		} else {
			timerSpecIdle = 0;
		}

		AnimatorTransitionInfo transInfo = anim.GetAnimatorTransitionInfo (0);
		if (transInfo.fullPathHash == idleTransIdleBack) {
			anim.SetBool ("IsFacingRight", false);
			timerSpecIdle = 0;
		}

		if (transInfo.fullPathHash == idleBackTransIdle) {
			anim.SetBool ("IsFacingRight", true);
			timerSpecIdle = 0;
		}

		if (stateInfo.fullPathHash == walkStateHash || stateInfo.fullPathHash == runStateHash ||
			stateInfo.fullPathHash == walkBackStateHash || stateInfo.fullPathHash == runBackStateHash) {
			Vector3 rot = transform.eulerAngles;
			rot.y = 90f;
			transform.eulerAngles = rot;
		}
		
		float speed = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", speed);

		//Run Control
		if (Input.GetKey (KeyCode.LeftShift)) {
			anim.SetBool ("Shift", true);
		} else {
			anim.SetBool ("Shift", false);
		}

		//Block Control
		if (Input.GetKey (KeyCode.F)) {
			anim.SetBool ("IsShielding", true);
		} else {
			anim.SetBool ("IsShielding", false);
		}
	}
}
