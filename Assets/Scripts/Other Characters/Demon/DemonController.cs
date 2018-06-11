using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour {
	Animator anim;
	GameObject thirang;
	Thirang th;
	Demon demon;
	public GameObject magicBall;

	public float distDemonPlayerAttack;
	public float distDemonPlayerSpell;

	bool deathStart;
	bool magicAttackSpawned;
	bool isFar;

	float timerAction;

	public bool isFacingLeft { get; set; }
	public bool isAttacking { get; set; }

	private readonly int spellCastHash = Animator.StringToHash ("Base Layer.Spell");
	private readonly int spellCastBackHash = Animator.StringToHash ("Base Layer.Back.Spell Back");

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		demon = this.gameObject.GetComponent<Demon> ();
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

		timerAction -= Time.deltaTime;

		//Adjust Archer position and rotation
		transform.position = new Vector3 (transform.position.x, transform.position.y);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);

		if (!demon.ThirangOnCycloneSpin()) {			//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
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
			if (offset < distDemonPlayerAttack &&
			   stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
			   stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
			{
				anim.SetBool ("Run", false);
				anim.SetTrigger ("Attack");
			} else {
				if (offset >= distDemonPlayerAttack && offset <= distDemonPlayerSpell) {
					anim.SetBool ("Run", true);
					anim.ResetTrigger ("Attack");
					anim.ResetTrigger ("Cast");
					isFar = false;
				} else if (!isFar && timerAction <= 1f) {
					int rand = Random.Range (0, 2);
					switch (rand) {
						case 0:
							anim.SetBool ("Run", true);
							anim.ResetTrigger ("Attack");
							anim.ResetTrigger ("Cast");
							isFar = true;
							break;
						case 1:
							anim.SetBool ("Run", false);
							anim.SetTrigger ("Cast");
							timerAction = 2f;
							break;
						default:
							throw new System.EntryPointNotFoundException ("Random Demon action error");
					}
				}
			}
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
				print ("pezzente");
			}
		} else {
			magicAttackSpawned = false;
		}

		//Death
		if (demon.health <= 0 && !deathStart) 
		{
			anim.SetTrigger ("Death");
			deathStart = true;
			demon.OnDeath ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				demon.fadingDeath = true;
			}
		}

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

		_magicAttack.demon = this.demon;
		_magicAttack.Direction (direction);
	}

	public bool OnFrameToHit() {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash)
			return stateInfo.normalizedTime > 0.45f && stateInfo.normalizedTime < 0.65f;

		return false;
	}
}
