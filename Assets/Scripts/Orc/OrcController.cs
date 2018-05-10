using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour {
	Animator anim;
	GameObject thirang;
	Orc orc;

	public float distOrcPlayer;

	public bool isAttacking { get; set; }
	bool deathStart;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		orc = this.gameObject.GetComponent<Orc> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		anim.SetBool ("Run", true);

		float dist = transform.position.x - thirang.transform.position.x;

		if (dist >= 0)
			anim.SetBool ("IsFacingLeft", true);
		if (dist < 0)
			anim.SetBool ("IsFacingLeft", false);
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		float dist = transform.position.x - thirang.transform.position.x;
		float offset = Mathf.Abs(dist);

		//Adjust Orc position and rotation
		transform.position = new Vector3 (transform.position.x, transform.position.y);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);

		if (dist >= 0)
			anim.SetBool ("IsFacingLeft", true);
		if (dist < 0)
			anim.SetBool ("IsFacingLeft", false);

		//Attack
		//Attack-Run state switching control
		if (offset < distOrcPlayer &&
			stateInfo.fullPathHash != EnemySaT.attackStateHash && stateInfo.fullPathHash != EnemySaT.attackBackStateHash &&
			stateInfo.fullPathHash != EnemySaT.reactionHitStateHash && stateInfo.fullPathHash != EnemySaT.reactionHitBackStateHash) 
		{
			anim.SetBool ("Run", false);
			anim.SetTrigger ("Attack");
		} else {
			if (offset > distOrcPlayer) {
				anim.SetBool ("Run", true);
				anim.ResetTrigger ("Attack");
			}
		}

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
			isAttacking = true;
		} else {
			isAttacking = false;
		}

		//Death
		if (orc.health <= 0 && !deathStart) 
		{
			anim.SetTrigger ("Death");
			deathStart = true;
			orc.OnDeath ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				orc.fadingDeath = true;
			}
		}

		//Reaction Hit
		if (stateInfo.fullPathHash == EnemySaT.reactionHitStateHash || stateInfo.fullPathHash == EnemySaT.reactionHitBackStateHash) {
			orc.startHit = false;
			Vector3 newPos = transform.position;
			//Push a bit back the enemy to permit Thirang second slash to hit him
			if (stateInfo.length - stateInfo.normalizedTime < 1) {
				newPos.x += anim.GetBool ("IsFacingLeft") ? 0.01f : -0.01f;
			}
			transform.position = newPos;
		}
	}
}