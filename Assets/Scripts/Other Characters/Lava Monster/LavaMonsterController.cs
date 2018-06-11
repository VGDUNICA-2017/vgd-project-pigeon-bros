using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonsterController : MonoBehaviour {
	Animator anim;
	GameObject thirang;
	Thirang th;
	LavaMonster lavaMonster;

	public float distLavaMonsterPlayerAttack;
	public float distJumpAttack;

	bool deathStart;
	bool isChangedAttack;
	bool jumpAttackLock;
	bool canJumpAttack;

	public bool isFacingLeft { get; set; }
	public bool isAttacking { get; set; }

	private readonly int mirrorAttackStateHash = Animator.StringToHash ("Base Layer.Mirror Attack");
	private readonly int mirrorAttackBackStateHash = Animator.StringToHash ("Base Layer.Back.Mirror Attack Back");
	private readonly int jumpAttackStateHash = Animator.StringToHash ("Base Layer.Jump Attack");
	private readonly int jumpAttackBackStateHash = Animator.StringToHash ("Base Layer.Back.Jump Attack Back");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		lavaMonster = GetComponent<LavaMonster> ();
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

		if (!lavaMonster.ThirangOnCycloneSpin()) {			//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
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
			if (offset < distLavaMonsterPlayerAttack &&
			   stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
			   stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
			{
				anim.SetBool ("Run", false);
				anim.SetTrigger ("Attack");
			} else {
				if (offset < distJumpAttack && canJumpAttack &&
				   (stateInfo.fullPathHash == EnemySaT.runStateHash || stateInfo.fullPathHash == EnemySaT.runBackStateHash)) {
					canJumpAttack = false;
					anim.SetTrigger ("JumpAttack");
				} else {
					if (offset > distJumpAttack)
						canJumpAttack = true;
				
					if (offset > distLavaMonsterPlayerAttack) {
						anim.SetBool ("Run", true);
						anim.ResetTrigger ("Attack");
					}
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
				lavaMonster.OnMirrorAttack ();
			}

			isAttacking = true;
		} else {
			isAttacking = false;
			isChangedAttack = false;
			jumpAttackLock = false;
		}

		//Death
		if (lavaMonster.health <= 0 && !deathStart) 
		{
			anim.SetTrigger ("Death");
			deathStart = true;
			lavaMonster.OnDeath ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				lavaMonster.fadingDeath = true;
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
