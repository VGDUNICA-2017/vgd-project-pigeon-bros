using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : EnemyController {
	public GameObject arrow;

	bool arrowSpawned;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		enemy = this.gameObject.GetComponent<Archer> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		th = thirang.GetComponent<Thirang> ();

		anim.SetBool ("Run", true);

		UpdatePosition ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (!enemy.ThirangOnCycloneSpin()) {	//Prevents a bug: during Thirang's Cyclone Spin the enemy turns around because Thirang moves back of him
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

		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
			if (stateInfo.normalizedTime > 0.85f && !arrowSpawned) {
				arrow.SetActive (false);
				SpawnArrow ();
				arrowSpawned = true;
			}

			if (stateInfo.normalizedTime > 0.32f && !arrowSpawned)
				arrow.SetActive (true);
			
		} else {
			arrowSpawned = false;
		}

		//Death
		if (!deathStart) 
		{
			EnemyDead ();
		}

		DeathAnimation (stateInfo);

	}

	void SpawnArrow() {
		Vector3 newRot;
		int direction;

		if (anim.GetBool ("IsFacingLeft")) {
			direction = -1;
			newRot = new Vector3 (0, 270);
		} else {
			direction = 1;
			newRot = new Vector3 (0, 90);
		}

		//Instantiates new Arrow and sets the direction where it must travel
		OnArrowHit newArrow = Instantiate (arrow, arrow.transform.position, Quaternion.Euler (newRot), null).AddComponent<OnArrowHit> ();
		newArrow.gameObject.tag = "Destroyable";
		newArrow.gameObject.transform.localScale = new Vector3 (5f, 5f, 2.5f);
		newArrow.gameObject.SetActive (true);
		newArrow.archer = enemy as Archer;
		newArrow.Direction (direction);
	}
}
