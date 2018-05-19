using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingLight : MonoBehaviour {
	float xOffset, yOffset, zStart;

	void Awake () {
		xOffset = Mathf.Abs (transform.position.x - transform.parent.position.x);
		yOffset = Mathf.Abs (transform.position.y - transform.parent.position.y);
		zStart = transform.position.z;
	}

	void LateUpdate () {
		Vector3 newPos = transform.position;
		print (transform.position);
		newPos.x = transform.parent.position.x + xOffset;
		newPos.y = transform.parent.position.y + yOffset;
		newPos.z = zStart;
		transform.position = newPos;
	}
}
