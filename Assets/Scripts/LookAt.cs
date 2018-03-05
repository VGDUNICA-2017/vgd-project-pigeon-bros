using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
	public Transform target;
	float xOffset, yOffset;

	void Start () {
		xOffset = Mathf.Abs(target.position.x - transform.position.x);
		yOffset = Mathf.Abs (target.position.y - transform.position.y);
	}

	// Update is called once per frame
	void LateUpdate () {
		Vector3 newPos = transform.position;
		newPos.x = target.position.x + xOffset;
		transform.position = newPos;

		Vector3 newHeight = transform.position;
		newHeight.y = target.position.y + yOffset;
		transform.position = newHeight;
	}
}
