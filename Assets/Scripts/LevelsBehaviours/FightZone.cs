using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightZone : MonoBehaviour {
	public int mages;
	public int fighters;
	GameObject th;

	float vertExtent, horzExtent;
	float minX, maxX;

	BoxCollider c;

	bool isDone;

	// Use this for initialization
	void Start () {
		vertExtent = Camera.main.orthographicSize;    
		horzExtent = vertExtent * Screen.width / Screen.height;

		c = GetComponent<Collider> () as BoxCollider;
	}
	
	// Update is called once per frame
	void LateUpdate () {

	}

	void OnTriggerEnter (Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			th = other.transform.root.gameObject;

			Camera.main.GetComponent <LookAt> ().enabled = false;
			maxX = horzExtent - th.transform.position.x / 2f;
			minX = -maxX;

			if (!isDone) {
				GameObject g = new GameObject ("LeftBound");
				BoxCollider cLeft = g.AddComponent<BoxCollider> ();
				cLeft.size = c.size;
				cLeft.isTrigger = false;
				g.transform.parent = transform;
				Vector3 newPos = new Vector3 (minX, 0, 0);
				g.transform.localPosition = newPos;

				GameObject g2 = new GameObject ("RightBound");
				BoxCollider cRight = g2.AddComponent<BoxCollider> ();
				cRight.size = c.size;
				cRight.isTrigger = false;
				g2.transform.parent = transform;
				newPos = new Vector3 (maxX, 0, 0);
				g2.transform.localPosition = newPos;
				isDone = true;
			}
		}
	}
}
