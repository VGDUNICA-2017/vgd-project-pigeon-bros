using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingSign : MonoBehaviour {

	public GameObject pointed;
	const float defSignZ = -181.107f;
	Transform trsParent;
	Vector3 diff;
	float angle;


	// Use this for initialization
	void Start () {
		trsParent = transform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
		diff = pointed.transform.position - trsParent.position;
		angle = Vector3.Angle (Vector3.right, diff);
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, defSignZ + angle);
	}
}
