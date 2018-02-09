using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour {
	public enum Axis {
		x,y
	}

	public Axis current;
	public float speed;
	public float offset;
	public bool backward;
	float start, end;
	Vector3 newPos;

	// Use this for initialization
	void Start () {
		if (current == Axis.x)
			start = transform.position.x;
		if (current == Axis.y)
			start = transform.position.y;

		end = start + offset;
		newPos = transform.position;
		if (backward) {
			start = start - offset;
			end = end - offset;
			speed = -speed;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (current == Axis.x) {
			if (transform.position.x > end || transform.position.x < start) {
				speed = -speed;
			}
				
			if (transform.position.x > end) {
				newPos.x = end;
				transform.position = newPos;
			} 
			if (transform.position.x < start) {
				newPos.x = start;
				transform.position = newPos;
			}


			newPos.x += speed * Time.deltaTime;
			transform.position = newPos;
		}

		if (current == Axis.y) {
			if (transform.position.y > end || transform.position.y < start) {
				speed = -speed;
			}

			if (transform.position.y > end) {
				newPos.y = end;
				transform.position = newPos;
			} 
			if (transform.position.y < start) {
				newPos.y = start;
				transform.position = newPos;
			}


			newPos.y += speed * Time.deltaTime;
			transform.position = newPos;
		}
	}
}
