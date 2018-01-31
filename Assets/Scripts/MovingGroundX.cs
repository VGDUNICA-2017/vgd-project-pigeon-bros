using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGroundX : MonoBehaviour {
	public float speed = 2f;
	public float offset = 32f;
	public bool backward = false;
	float start, end;
	Vector3 newPos;

	// Use this for initialization
	void Start () {
		start = transform.position.x;
		end = start + offset;
		newPos = transform.position;
		if (backward) {
			var x = start;
			start = end;
			end = x;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > end || transform.position.x < start) {
			speed *= -1;
		}

		newPos.x += speed * Time.deltaTime;
		transform.position = newPos;
	}
}
