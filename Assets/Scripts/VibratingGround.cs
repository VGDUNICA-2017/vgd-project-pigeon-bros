using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibratingGround : MonoBehaviour {

	bool toVibrate, toFallDown, toRaiseUp, startCount;
	float timer;

	int nVibes = 8;
	public float speedVibe;
	public float offsetVibe;
	Vector3 center;

	public float distanceFallRaise;
	public float speedFall;
	public float speedRaise;
	float downFallY;

	public float waitRaise;

	// Use this for initialization
	void Start () {
		toVibrate = false;
		toFallDown = false;
		toRaiseUp = false;
		startCount = false;
		timer = 0f;

		center = transform.position;

		downFallY = center.y - distanceFallRaise;
	}
	
	// Update is called once per frame
	void Update () {
		if (startCount) {
			timer += Time.deltaTime;
		}

		if (toVibrate) {
			Vibrate ();
		}

		if (toFallDown) {
			FallDown ();
		}

		if (toRaiseUp && timer >= waitRaise) {
			RaiseUp ();
		}
	}

	void Vibrate() {
		if (nVibes > 0) {
			Vector3 newPos = transform.position;
			newPos.y += speedVibe * Time.deltaTime;
			transform.position = newPos;

			if (transform.position.y >= center.y + offsetVibe) {
				newPos.y = center.y + offsetVibe;
				transform.position = newPos;
				speedVibe = -speedVibe;
			}

			if (transform.position.y <= center.y - offsetVibe) {
				newPos.y = center.y - offsetVibe;
				transform.position = newPos;
				speedVibe = -speedVibe;
				nVibes--;
			}
		} else {
			nVibes = 8;
			toVibrate = false;
			toFallDown = true;
		}
	}

	void FallDown() {
		Vector3 newPos = transform.position;

		if (transform.position.y <= downFallY) {
			toFallDown = false;
			toRaiseUp = true;
			startCount = true;
		} else {
			newPos.y -= speedFall * Time.deltaTime;
			transform.position = newPos;
		}
	}

	void RaiseUp() {
		Vector3 newPos = transform.position;

		if (transform.position.y >= center.y) {
			transform.position = center;
			toRaiseUp = false;
			startCount = false;
			timer = 0f;
		} else {
			newPos.y += speedRaise * Time.deltaTime;
			transform.position = newPos;
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Player") {
			toVibrate = true;
		}
	}
}
