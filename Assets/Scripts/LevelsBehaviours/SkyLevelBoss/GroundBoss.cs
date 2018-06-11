using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBoss : MonoBehaviour {
	public GameObject mage;

	public bool attackState { get; set; }

	public float speed;
	public float start;
	public float end;

	bool toMove = true, timerStarted;
	Vector3 newPos, magePos;

	bool thirangOnBossGround;

	// Use this for initialization
	void Start () {
		newPos = transform.position;
		magePos = mage.transform.position;
	}

	void LateUpdate() {
		if (toMove)
			Move ();
		else if (!timerStarted) {
			if (attackState) {
				StartCoroutine (Wait (25f));
				mage.GetComponent<Mage> ().bossFightHit = false;
			}
			else
				StartCoroutine (Wait (10f));

			timerStarted = true;
		}

		if (attackState && thirangOnBossGround)
			GameObject.FindObjectOfType <Thirang> ().FatalDamage ();
	}

	void Move() {
		if (thirangOnBossGround)
			GameObject.FindObjectOfType <Thirang> ().FatalDamage ();

		if (transform.position.x > end || transform.position.x < start) {
			speed = -speed;
		}

		newPos.x += speed * Time.deltaTime;
		magePos.x += speed * Time.deltaTime;

		if (transform.position.x > end) {
			newPos.x = end;
			transform.position = newPos;
			attackState = true;
			toMove = false;
		} 
		if (transform.position.x < start) {
			newPos.x = start;
			transform.position = newPos;
			attackState = false;
			toMove = false;
		}

		transform.position = newPos;
		mage.transform.position = magePos;
	}

	IEnumerator Wait(float time) {
		yield return new WaitForSecondsRealtime (time);
		toMove = true;
		timerStarted = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player"))
			thirangOnBossGround = true;
	}

	void OnTriggerExit(Collider other) {
		if (other.transform.root.CompareTag ("Player"))
			thirangOnBossGround = false;
	}
}	
