using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : EnemyController {
	public GameObject magicBall;
	GroundRaycast[] rays;

	public float distDemonPlayerSpell;

	bool magicAttackSpawned;
	bool isFar;

	float timerAction;

	private readonly int spellCastHash = Animator.StringToHash ("Base Layer.Spell");
	private readonly int spellCastBackHash = Animator.StringToHash ("Base Layer.Back.Spell Back");

	public static readonly int startStateHash = Animator.StringToHash ("Base Layer.Start");

	void Awake() {
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);
	}

	void Start () {
		anim = GetComponent <Animator> ();
		enemy = this.gameObject.GetComponent<Demon> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		th = thirang.GetComponent<Thirang> ();

		th.onEarthBossFight = true;

		rays = thirang.GetComponentsInChildren <GroundRaycast> ();

		UpdatePosition ();
	}

	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		timerAction -= Time.deltaTime;

		if (!enemy.ThirangOnCycloneSpin()) {			//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
			UpdatePosition();
		}

		//Attack
		//Attack-Run state switching control
		if (!rays [0].onMovingGround || !rays [1].onMovingGround) {
			if (thirangDistance < distanceThreshold &&
			    stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
			    stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
			{
				anim.SetBool ("Run", false);
				anim.SetTrigger ("Attack");
			} else {
				if (thirangDistance >= distanceThreshold && thirangDistance <= distDemonPlayerSpell) {
					anim.SetBool ("Run", true);
					anim.ResetTrigger ("Attack");
					anim.ResetTrigger ("Cast");
					isFar = false;
				} else if (!isFar && timerAction <= 1f) {
					int rand = Random.Range (1, 21);
					switch (rand <= 10) {
						case true:
							anim.SetBool ("Run", true);
							anim.ResetTrigger ("Attack");
							anim.ResetTrigger ("Cast");
							isFar = true;
							break;
						case false:
							anim.SetBool ("Run", false);
							anim.SetTrigger ("Cast");
							timerAction = 2f;
							break;
						default:
							throw new System.EntryPointNotFoundException ("Random Demon action error");
					}
				}
			}
		} else {
			anim.SetBool ("Run", false);
			anim.SetTrigger ("Cast");
		}

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
			isAttacking = true;
		} else {
			isAttacking = false;
		}

		if (stateInfo.fullPathHash == spellCastHash || stateInfo.fullPathHash == spellCastBackHash) {
			if (stateInfo.normalizedTime > 0.51f && !magicAttackSpawned) {
				SpellCast ();
				magicAttackSpawned = true;
			}
		} else {
			magicAttackSpawned = false;
		}

		//Death
		if (!deathStart) 
		{
			EnemyDead ();
		}
		
		//if dead
		DeathAnimation (stateInfo, "LivelloMare");
	}

	void SpellCast() {
		int direction;

		if (anim.GetBool ("IsFacingLeft")) {
			direction = -1;
		} else {
			direction = 1;
		}

		OnMagicAttackHit _magicAttack = Instantiate (magicBall, transform.GetChild (transform.childCount - 1).position, 
			Quaternion.identity, null).AddComponent<OnMagicAttackHit> ();

		_magicAttack.demon = enemy as Demon;
		_magicAttack.Direction (direction);
	}

	public bool OnFrameToHit() {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash)
			return stateInfo.normalizedTime > 0.45f && stateInfo.normalizedTime < 0.65f;

		return false;
	}

	public void Teleport(int plane) {
		transform.position = new Vector3 (transform.position.x, transform.position.y + 11 * plane);
	}
}
