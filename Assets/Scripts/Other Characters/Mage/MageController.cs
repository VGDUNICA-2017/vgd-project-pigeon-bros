using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour {
	Animator anim;
	Thirang th;
	Mage mage;
	public GameObject magicBall;
	public GameObject ground;

	GroundBoss groundBoss;

	bool deathStart;
	bool magicAttackSpawned;

	public bool isFacingLeft { get; set; }

	bool newLevelSet;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		mage = this.gameObject.GetComponent<Mage> ();
		groundBoss = ground.GetComponent<GroundBoss> ();
		th = GameObject.FindGameObjectWithTag ("Player").GetComponent<Thirang> ();

		anim.SetBool ("IsFacingLeft", true);
		isFacingLeft = true;
	}

	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		//Adjust Mage position and rotation
		transform.position = new Vector3 (transform.position.x, transform.position.y);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);

		//Attack
		if (stateInfo.fullPathHash == EnemySaT.attackStateHash || stateInfo.fullPathHash == EnemySaT.attackBackStateHash) {
			if (stateInfo.normalizedTime > 0.35f && !magicAttackSpawned) {
				SpawnMagicBall ();
				magicAttackSpawned = true;
			}

		} else {
			magicAttackSpawned = false;

			if (groundBoss.attackState && !th.isDead)
				anim.SetTrigger ("Attack");
		}

		//Death
		if (mage.health <= 0 && !deathStart) 
		{
			anim.SetTrigger ("Death");
			deathStart = true;
			mage.OnDeath ();
		}

		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				anim.enabled = false;	//Executed when Death animation will finish
				mage.fadingDeath = true;
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

		_magicBall.mage = mage;
		_magicBall.Direction (direction);
	}
}
