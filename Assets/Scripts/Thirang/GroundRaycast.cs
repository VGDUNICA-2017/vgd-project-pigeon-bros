using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRaycast : MonoBehaviour {
	Animator anim;
	Transform thTransform;

	//Avoid multiple settings of trigger "JumpDown" caused by multiple calls of FixedUpdate
	bool isLanding;

	public bool onGround { get; set; }
	public bool onMovingGround { get; set; }
	float timeFalling;

	// Use this for initialization
	void Start () {
		anim = GetComponentInParent <Animator> ();
		thTransform = transform.root;
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
				     hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround") ||
					 hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava1") ||
				 	 hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava2") ||
					 hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava3"))) 
				{
					anim.SetTrigger ("JumpDown");
					isLanding = true;
				}
					
			}

			//Moving Grund
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround") && hit.distance <= 1.8f) {
				thTransform.SetParent (hit.collider.gameObject.transform);
				onMovingGround = true;
			} else {
				if (onMovingGround) 
					NotOnMovingGround ();
			}

			if (anim.GetBool ("IsJumping") && onMovingGround) {
				NotOnMovingGround ();
			}

			if (hit.distance <= 1.2f &&
			    (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground") ||
			    hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround") ||
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava1") ||
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava2") ||
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava3"))) 
			{
				onGround = true;
			}
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

	void NotOnMovingGround() {
		onMovingGround = false;
		thTransform.SetParent (null);
	}
}