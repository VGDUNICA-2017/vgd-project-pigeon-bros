using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCamera : MonoBehaviour {

	float offsetX, offsetY;
	Vector3 newPos, posCam;

	// Use this for initialization
	void Start () {
		offsetX = Mathf.Abs(Camera.main.transform.position.x - transform.position.x);
		offsetY = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		posCam = Camera.main.transform.position;
		newPos = new Vector3 (posCam.x + offsetX, posCam.y + offsetY, 0);
		transform.position = newPos;
	}
}
