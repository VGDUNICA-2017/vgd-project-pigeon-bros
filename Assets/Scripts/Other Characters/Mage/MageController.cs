using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : EnemyController {
	public GameObject magicBall;
	public GameObject ground;

	GroundBoss groundBoss;

	bool magicAttackSpawned;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		enemy = this.gameObject.GetComponent<Mage> ();
		groundBoss = ground.GetComponent<GroundBoss> ();

		anim.SetBool ("IsFacingLeft", true);
		isFacingLeft = true;
	}

	// Update is called once per frame
	void Update () {
		distanceThreshold = float.PositiveInfinity;
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		//Attack
		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
			if (stateInfo.normalizedTime > 0.35f && !magicAttackSpawned) {
				SpawnMagicBall ();
				magicAttackSpawned = true;
			}

		} else {
			magicAttackSpawned = false;

			if (groundBoss.attackState)
				anim.SetTrigger ("Attack");
		}

		//Death
		if (!deathStart) 
		{
			EnemyDead ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				enemy.fadingDeath = true;
				if (!newLevelSet) {
					this.gameObject.AddComponent<TeleportBehaviour> ().BossKilled ("LivelloTerra");
					newLevelSet = true;
				}
			}
		}

	}

	void SpawnMagicBall() {
		int direction;

		if (anim.GetBool ("IsFacingLeft")) {
			direction = -1;
		} else {
			direction = 1;
		}

		Vector3 pos1 = transform.GetChild (transform.childCount - 1).position;
		Vector3 pos2 = pos1;
		Vector3 pos3 = pos1;
		pos2.y += 8f;
		pos3.y -= 8f;

		Vector3[] positions = { pos1, pos2, pos3 };

		for (int i = 0; i < positions.Length; i++) {
			Vector3 p = positions [i];
			p.z = 0;

			StartCoroutine (Spawn (p, direction));
		}
	}

	IEnumerator Spawn(Vector3 p, int direction) {
		yield return new WaitForSeconds (Random.Range (0f, 1f));
		OnMagicBallHit _magicBall = Instantiate (magicBall, p, 
			Quaternion.identity, null).GetComponent<OnMagicBallHit> ();

		_magicBall.mage = enemy as Mage;
		_magicBall.Direction (direction);
	}
}
