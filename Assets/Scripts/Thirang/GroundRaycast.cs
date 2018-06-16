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
	public bool onDemonUpperGround { get; set; }
	public bool onDemonLowerGround { get; set; }

	int layerMask;

	// Use this for initialization
	void Start () {
		anim = GetComponentInParent <Animator> ();
		thTransform = transform.root;

		LayerMaskInit ();
	}

	void FixedUpdate () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		RaycastHit hit;

		if (Physics.Raycast (transform.position + new Vector3 (0, 0.1f, 0), Vector3.down, out hit, Mathf.Infinity, layerMask) ||
			Physics.Raycast (transform.position + new Vector3 (0.7f, 0.1f, 0), Vector3.down, out hit, Mathf.Infinity, layerMask) ||
			Physics.Raycast (transform.position - new Vector3 (0.7f, 0.1f, 0), Vector3.down, out hit, Mathf.Infinity, layerMask)) 
		{	
			if (stateInfo.fullPathHash == ThirangSaT.jumpIdle || stateInfo.fullPathHash == ThirangSaT.jumpBackIdle) {

				if ((!isLanding && hit.distance <= 1.8f) &&
				    (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground") ||
				     hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround") ||
					 hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava1") ||
				 	 hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava2") ||
					 hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava3") ||
					 hit.collider.gameObject.layer == LayerMask.NameToLayer ("DemonUpperGround"))) 
				{
					anim.SetTrigger ("JumpDown");
					isLanding = true;
				}
					
			}

			//Moving Grund
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround") && hit.distance <= 2f) {
				thTransform.SetParent (hit.collider.gameObject.transform);
				onMovingGround = true;
			} else {
				if (onMovingGround) 
					NotOnMovingGround ();
			}

			if (anim.GetBool ("IsJumping") && onMovingGround) {
				NotOnMovingGround ();
			}

			if (hit.distance <= 1.5f &&
			    (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Ground") ||
			    hit.collider.gameObject.layer == LayerMask.NameToLayer ("MovingGround") ||
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava1") ||
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava2") ||
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("GroundLava3")) ||
				hit.collider.gameObject.layer == LayerMask.NameToLayer ("DemonUpperGround")) 
			{
				onGround = true;
			}
			else {
				onGround = false;
			}

		} else {
			onGround = false;
		}
	}

	public void Landed () {
		isLanding = false;
	}

	void NotOnMovingGround() {
		onMovingGround = false;
		thTransform.SetParent (null);
	}

	void LayerMaskInit() {
		layerMask = 1 << 8;
		layerMask |= 1 << 10;
		layerMask |= 1 << 11;
		layerMask |= 1 << 12;
		layerMask |= 1 << 13;
		layerMask |= 1 << 17;
	}
}