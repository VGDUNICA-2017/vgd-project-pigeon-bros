using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRaycast : MonoBehaviour {
	Animator anim;

	//Avoid multiple settings of trigger "JumpDown" caused by multiple calls of FixedUpdate
	bool isLanding;

	public bool onGround { get; set; }
	float timeFalling;

	// Use this for initialization
	void Start () {
		anim = GetComponentInParent <Animator> ();
	}

	void FixedUpdate () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		RaycastHit hit;

		if (Physics.Raycast (transform.position, Vector3.down, out hit, Mathf.Infinity) ||
		    Physics.Raycast (transform.position + new Vector3 (0.7f, 0, 0), Vector3.down, out hit, Mathf.Infinity) ||
		    Physics.Raycast (transform.position - new Vector3 (0.7f, 0, 0), Vector3.down, out hit, Mathf.Infinity)) 
		{	
			if (stateInfo.fullPathHash == ThirangSaT.jumpIdle || stateInfo.fullPathHash == ThirangSaT.jumpBackIdle) {

				if ((!isLanding && hit.distance <= 1.8f) &&
				    (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground") ||
				    hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround"))) 
				{
					anim.SetTrigger ("JumpDown");
					isLanding = true;
				}

				if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround") && hit.distance <= 1f) {
					transform.root.SetParent (hit.collider.gameObject.transform);
				} else {
					transform.root.SetParent (null);
				}
			}

			if (hit.distance <= 1.2f && 
			   (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground") || 
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround")))
				onGround = true;
			else {
				onGround = false;
			}

			timeFalling = 0;
		} else {
			onGround = false;
			timeFalling += Time.deltaTime;
			if (timeFalling >= 2f) {
				//Animation Falling
			}
		}
	}

	public void Landed () {
		isLanding = false;
	}
}