using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour {
	public enum Axis {
		x,y
	}

	public enum State {
		MovingUp, MovingDown, MovingLeft, MovingRight
	}

	public Axis axis;
	public float speed;
	public float offset;
	public bool backward;	//It refers just of the start of the movement of ground
	[HideInInspector] public State state;
	float start, end;
	Vector3 newPos;

	// Use this for initialization
	void Start () {
		if (axis == Axis.x)
			start = transform.position.x;
		if (axis == Axis.y)
			start = transform.position.y;

		end = start + offset;
		newPos = transform.position;
		if (backward) {
			start = start - offset;
			end = end - offset;
			speed = -speed;
		}

		if (axis == Axis.x) {
			if (backward)
				state = State.MovingLeft;
			else
				state = State.MovingRight;
		}

		if (axis == Axis.y) {
			if (backward)
				state = State.MovingDown;
			else
				state = State.MovingUp;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (axis == Axis.x) {
			if (transform.position.x > end || transform.position.x < start) {
				speed = -speed;

				if (state == State.MovingLeft)
					state = State.MovingRight;
				else 
					if (state == State.MovingRight)
						state = State.MovingLeft;
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

		if (axis == Axis.y) {
			if (transform.position.y > end || transform.position.y < start) {
				speed = -speed;

				if (state == State.MovingUp)
					state = State.MovingDown;
				else 
					if (state == State.MovingDown)
						state = State.MovingUp;
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
