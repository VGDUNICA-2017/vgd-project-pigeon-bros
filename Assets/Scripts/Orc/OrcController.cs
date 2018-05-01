using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonoBehaviour {
	Animator anim;
	GameObject thirang;
	Orc orc;

	public float distOrcPlayer;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		orc = this.gameObject.GetComponent<Orc> ();
		thirang = GameObject.FindGameObjectWithTag ("Player");
		anim.SetBool ("Run", true);
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		float dist = transform.position.x - thirang.transform.position.x;
		float offset = Mathf.Abs(dist);

		transform.position = new Vector3 (transform.position.x, transform.position.y);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90, transform.eulerAngles.z);

		if (dist >= 0)
			anim.SetBool ("IsFacingLeft", true);
		if (dist < 0)
			anim.SetBool ("IsFacingLeft", false);
			

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

		if (orc.health <= 0 &&
			stateInfo.fullPathHash != EnemySaT.deathStateHash && stateInfo.fullPathHash != EnemySaT.deathBackStateHash) 
		{
			anim.SetTrigger ("Death");
		}

		if (stateInfo.fullPathHash == EnemySaT.reactionHitStateHash || stateInfo.fullPathHash == EnemySaT.reactionHitBackStateHash) {
			orc.startHit = false;
		}
	}
}