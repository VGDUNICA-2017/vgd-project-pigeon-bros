using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonsterController : EnemyController {
	public float distJumpAttack;

	bool isChangedAttack;
	bool jumpAttackLock;
	bool canJumpAttack;

	private readonly int mirrorAttackStateHash = Animator.StringToHash ("Base Layer.Mirror Attack");
	private readonly int mirrorAttackBackStateHash = Animator.StringToHash ("Base Layer.Back.Mirror Attack Back");
	private readonly int jumpAttackStateHash = Animator.StringToHash ("Base Layer.Jump Attack");
	private readonly int jumpAttackBackStateHash = Animator.StringToHash ("Base Layer.Back.Jump Attack Back");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		enemy = GetComponent<LavaMonster> ();
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
			if (thirangDistance < distJumpAttack && canJumpAttack &&
			   (stateInfo.fullPathHash == EnemySaT.runStateHash || stateInfo.fullPathHash == EnemySaT.runBackStateHash)) {
				canJumpAttack = false;
				anim.SetTrigger ("JumpAttack");
			} else {
				if (thirangDistance > distJumpAttack)
					canJumpAttack = true;
			
				if (thirangDistance > distanceThreshold) {
					anim.SetBool ("Run", true);
					anim.ResetTrigger ("Attack");
				}
			}
		}

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash ||
			stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash ||
			stateInfo.fullPathHash == jumpAttackStateHash || stateInfo.fullPathHash == jumpAttackBackStateHash) 
		{
			if (!isChangedAttack && 
				(stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash)) {
				isChangedAttack = true;
				((LavaMonster)enemy).OnMirrorAttack ();
			}

			isAttacking = true;
		} else {
			isAttacking = false;
			isChangedAttack = false;
			jumpAttackLock = false;
		}

		//Death
		if (!deathStart) 
		{
			EnemyDead ();
		}
			
		DeathAnimation (stateInfo);

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				GameObject.Find ("DialogDispatcher").SetActive (true);
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

	public bool OnJumpAttack() {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		return stateInfo.fullPathHash == jumpAttackStateHash || stateInfo.fullPathHash == jumpAttackBackStateHash;
	}

	public bool OnJumpAttackHitControl() {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		if (!jumpAttackLock) {
			if (stateInfo.fullPathHash == jumpAttackStateHash || stateInfo.fullPathHash == jumpAttackBackStateHash) {
				jumpAttackLock = true;
				return true;
			}
		}

		return false;
	}

	public bool OnFrameToHit() {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash ||
			stateInfo.fullPathHash == mirrorAttackStateHash || stateInfo.fullPathHash == mirrorAttackBackStateHash)
			return stateInfo.normalizedTime > 0.45f && stateInfo.normalizedTime < 0.65f;

		if (stateInfo.fullPathHash == jumpAttackStateHash || stateInfo.fullPathHash == jumpAttackBackStateHash)
			return stateInfo.normalizedTime > 0.41f && stateInfo.normalizedTime < 0.61f;

		return false;
	}
}
