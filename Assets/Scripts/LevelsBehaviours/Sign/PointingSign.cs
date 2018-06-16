using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingSign : MonoBehaviour {

	public enum Direction
	{
		right,left
	}

	public enum Way
	{
		upward,downward
	}

	public Direction pointingDirection;
	public Way way;
	public GameObject pointed;

	float defSignZ;
	Transform trsParent;
	Vector3 diff;
	float angle;

	void Awake () {
		defSignZ = transform.eulerAngles.z;
	}

	// Use this for initialization
	void Start () {
		trsParent = transform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
		diff = pointed.transform.position - trsParent.position;
		if (pointingDirection == Direction.right) {
			angle = Vector3.Angle (Vector3.right, diff);
			if (way == Way.downward)
				angle = -angle;
		}
		if (pointingDirection == Direction.left) {
			angle = Vector3.Angle (Vector3.left, diff);
			if (way == Way.upward)
				angle = -angle;
		}
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, defSignZ + angle);
	}
}
