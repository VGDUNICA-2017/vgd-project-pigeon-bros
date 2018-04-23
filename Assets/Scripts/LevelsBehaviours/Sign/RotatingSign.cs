using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSign : MonoBehaviour {
	public const float Wait = 10f;
	public const float Back = 4f;

	float countTime;

	public float rotateSpeed;
	float startRotation, endRotation;
	bool toRotate, toRotateBack, isRotatedAndSwinged, toSwing, toSwingBack;

	public float swingSpeed;
	float swingStart, swingEnd;

	// Use this for initialization
	void Start () {
		toRotate = false;
		toRotateBack = false;
		isRotatedAndSwinged = false;
		toSwing = false;
		toSwingBack = false;

		startRotation = gameObject.transform.eulerAngles.y;
		endRotation = startRotation + 180f;

		countTime = 0f;

		swingStart = gameObject.transform.eulerAngles.z;
		swingEnd = swingStart - 37f;
	}
	
	// Update is called once per frame
	void Update () {
		/*if not visible by camera
		 * rot.setFromtoRotation(rot.eularAngles, new Vector3(0,0,-181.5f);
		 * countTime = 0f;
		 * else*/
		countTime += Time.deltaTime;

		if(!toRotate && !toRotateBack)
			TimeWait (countTime);

		if(toRotate)
			SignRotation (1);

		if (toSwing)
			SignSwing();

		if (toSwingBack)
			BackFromSwing ();

		if (toRotateBack)
			SignRotation (-1);
	}

	void TimeWait(float t) {
		if (t >= Wait)
			toRotate = true;
		
		if (isRotatedAndSwinged && t >= Back) {
			toSwingBack = true;
			isRotatedAndSwinged = false;
		}
	}

	void SignRotation(int way) {
		Vector3 newRot = gameObject.transform.eulerAngles;
		newRot.y += way * rotateSpeed * Time.deltaTime;
		gameObject.transform.eulerAngles = newRot;

		if (gameObject.transform.eulerAngles.y >= endRotation && toRotate) {
			newRot.y = endRotation;
			gameObject.transform.eulerAngles = newRot;
			toRotate = false;
			toSwing = true;
		}
			
		if (gameObject.transform.eulerAngles.y <= (startRotation + rotateSpeed*Time.deltaTime + 1f) && toRotateBack) {
			newRot.y = startRotation;
			gameObject.transform.eulerAngles = newRot;
			countTime = 0f;
			toRotateBack = false;
		}
	}

	void SignSwing() {
		Vector3 newRot = gameObject.transform.eulerAngles;
		newRot.z += swingSpeed * Time.deltaTime;
		
		if (newRot.z <= swingEnd) {
			newRot.z = swingEnd;
			gameObject.transform.eulerAngles = newRot;
			toSwing = false;
			countTime = 0f;
			swingSpeed = -swingSpeed;
			isRotatedAndSwinged = true;
		}
		else
			gameObject.transform.eulerAngles = newRot;
	}

	void BackFromSwing() {
		Vector3 newRot = gameObject.transform.eulerAngles;
		newRot.z += swingSpeed * Time.deltaTime;

		if (newRot.z >= swingStart) {
			newRot.z = swingStart;
			gameObject.transform.eulerAngles = newRot;
			toRotateBack = true;
			toSwingBack = false;
			swingSpeed = -swingSpeed;
		} else
			gameObject.transform.eulerAngles = newRot;
	}
}
