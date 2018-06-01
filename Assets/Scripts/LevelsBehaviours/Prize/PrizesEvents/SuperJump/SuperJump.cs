using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJump : MonoBehaviour {
	public RuntimeAnimatorController animCtrl;

	GameObject thirang;

	bool eventTriggered;

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !eventTriggered) {
			eventTriggered = true;

			thirang = other.transform.root.gameObject;

			thirang.GetComponent<ThirangController> ().enabled = false;

			thirang.GetComponent<Animator> ().SetFloat ("Speed", 0);
			thirang.GetComponent<Animator> ().CrossFade (ThirangSaT.idleStateHash, 0.4f);
			//GUI to say him to use SuperJump if available

			Jump ();
		}
	}

	void Jump() {
		GameObject g = new GameObject ();
		g.transform.position = new Vector3 (426f, -76.555f);

		g.AddComponent<Animator> ();
		Animator animG = g.GetComponent<Animator> ();
		animG.runtimeAnimatorController = animCtrl;

		thirang.transform.SetParent (g.transform);
		thirang.GetComponent<Rigidbody> ().isKinematic = true;

		animG.SetTrigger ("Jump");

		StartCoroutine (GetControlBack (g));
	}

	IEnumerator GetControlBack(GameObject g) {
		yield return new WaitForSecondsRealtime (2.2f);
		thirang.transform.SetParent (null);
		thirang.GetComponent<Rigidbody> ().isKinematic = false;
		thirang.GetComponent<ThirangController> ().enabled = true;

		Destroy	(g);
		Destroy (this.gameObject);
	}
}
