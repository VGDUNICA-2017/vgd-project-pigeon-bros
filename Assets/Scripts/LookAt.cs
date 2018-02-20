using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	Animator anim;
	readonly int idleStateHash = Animator.StringToHash ("Base Layer.Idle");
	readonly int idleSpecStateHash = Animator.StringToHash ("Base Layer.Special Idle");
	readonly int jumpStateHash = Animator.StringToHash ("Base Layer.Jump");
	public Transform target;
	float xOffset, yOffset;

	void Start () {
		xOffset = Mathf.Abs(target.position.x - transform.position.x);
		yOffset = Mathf.Abs (target.position.y - transform.position.y);
		anim = target.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void LateUpdate () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (stateInfo.fullPathHash != idleStateHash || stateInfo.fullPathHash != idleSpecStateHash) {
			Vector3 newPos = transform.position;
			newPos.x = target.position.x + xOffset;
			transform.position = newPos;
		}

		if (stateInfo.fullPathHash == jumpStateHash) {
			Vector3 newHeight = transform.position;
			newHeight.y = target.position.y + yOffset;
			transform.position = newHeight;
		}
	}
}
