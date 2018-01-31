using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraLvl01Boss : MonoBehaviour {
	public float speed = 8f;

	// Update is called once per frame
	void Update () {
		Vector3 currPosition = Camera.main.transform.position;
		currPosition.x -= speed*Time.deltaTime;
		Camera.main.transform.position = currPosition;
		if (currPosition.x < 27)
			this.enabled = false;
	}
}
