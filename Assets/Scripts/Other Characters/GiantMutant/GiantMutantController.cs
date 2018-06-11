using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantMutantController : MonoBehaviour {
	Animator anim;
	GameObject thirang;
	Thirang th;
	GiantMutant giantMutant;

	public float distGiantMutantPlayerAttack;

	bool deathStart;
	bool isChangedAttack;

	public bool isFacingLeft { get; set; }
	public bool isAttacking { get; set; }

	private readonly int mirrorAttackStateHash = Animator.StringToHash ("Base Layer.Mirror Attack");
	private readonly int mirrorAttackBackStateHash = Animator.StringToHash ("Base Layer.Back.Mirror Attack Back");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		giantMutant = GetComponent<GiantMutant> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		th = thirang.GetComponent<Thirang> ();

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

		//Adjust Archer position and rotation
		transform.position = new Vector3 (transform.position.x, transform.position.y);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);

		if (!giantMutant.ThirangOnCycloneSpin()) {			//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
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
		if (!th.isDead) {
			if (offset < distGiantMutantPlayerAttack &&
			   stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
			   stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
			{
				anim.SetBool ("Run", false);
				anim.SetTrigger ("Attack");
			} else {
				if (offset > distGiantMutantPlayerAttack) {
					anim.SetBool ("Run", true);
					anim.ResetTrigger ("Attack");
				}
			}
		}

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash ||
			stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash) 
		{
			if (!isChangedAttack && 
				(stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash)) {
				isChangedAttack = true;
				giantMutant.OnMirrorAttack ();
			}

			isAttacking = true;
		} else {
			isAttacking = false;
			isChangedAttack = false;
		}

		//Death
		if (giantMutant.health <= 0 && !deathStart) 
		{
			anim.SetTrigger ("Death");
			deathStart = true;
			giantMutant.OnDeath ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				giantMutant.fadingDeath = true;
			}
		}

	}

	public bool IsThisArmAttacking (string arm) {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		switch (arm) {
			case "left":
				if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash)
					return true;

				return false;
			case "right":
				if (stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash)
					return true;

				return false;
			default:
				return false;
		}
	}

	public bool OnFrameToHit() {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash ||
			stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash)
			return stateInfo.normalizedTime > 0.45f && stateInfo.normalizedTime < 0.65f;

		return false;
	}
}
