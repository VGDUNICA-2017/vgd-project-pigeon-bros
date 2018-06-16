using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFights : MonoBehaviour {
	int children;

	// Use this for initialization
	void Start () {
		children = transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.childCount < children)
			Destroy (this.gameObject);
	}
}
