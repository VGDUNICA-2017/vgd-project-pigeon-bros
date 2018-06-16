using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantMutantController : EnemyController {
	bool isChangedAttack;

	private readonly int mirrorAttackStateHash = Animator.StringToHash ("Base Layer.Mirror Attack");
	private readonly int mirrorAttackBackStateHash = Animator.StringToHash ("Base Layer.Back.Mirror Attack Back");

	public static readonly int startStateHash = Animator.StringToHash ("Base Layer.Start");

	void Awake() {
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		enemy = GetComponent<GiantMutant> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		th = thirang.GetComponent<Thirang> ();

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

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash ||
			stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash) 
		{
			if (!isChangedAttack && 
				(stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash)) {
				isChangedAttack = true;
				((GiantMutant)enemy).OnMirrorAttack ();
			}

			isAttacking = true;
		} else {
			isAttacking = false;
			isChangedAttack = false;
		}

		//Death
		if (!deathStart) 
		{
			EnemyDead ();
		}

		DeathAnimation (stateInfo, "LivelloFinale");

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
