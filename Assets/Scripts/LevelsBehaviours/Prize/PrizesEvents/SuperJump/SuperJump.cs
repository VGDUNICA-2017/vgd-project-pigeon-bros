using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJump : MonoBehaviour {
	public RuntimeAnimatorController animCtrl;
	public GameObject dialogDispatcher;

	GameObject thirang;
	Thirang th;

	bool eventTriggered1, eventTriggered2;

	void Start() {
		dialogDispatcher.GetComponent<DialogDispatcher> ().cantTrigger = true;
	}

	void OnTriggerEnter(Collider other) {
		if (other.transform.root.CompareTag ("Player") && !eventTriggered1) {
			eventTriggered1 = true;

			thirang = other.transform.root.gameObject;
			th = thirang.GetComponent<Thirang> ();

			thirang.GetComponent<ThirangController> ().enabled = false;

			thirang.GetComponent<Animator> ().SetFloat ("Speed", 0);
			thirang.GetComponent<Animator> ().CrossFade (ThirangSaT.idleStateHash, 0.4f);

			dialogDispatcher.GetComponent<DialogDispatcher> ().cantTrigger = false;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.transform.root.CompareTag ("Player")) {
			if (th.SpecialAbility (this.tag) && !eventTriggered2) {
				if (Input.GetButtonDown ("specialAbility")) {
					eventTriggered2 = true;
					Jump ();
				}
			}
		}
	}

	public void Jump() {
		GameObject g = new GameObject ();
		g.transform.position = new Vector3 (426f, -76.555f);

		g.AddComponent<Animator> ();
		Animator animG = g.GetComponent<Animator> ();
		animG.runtimeAnimatorController = animCtrl;

		thirang.transform.SetParent (g.transform);
		thirang.GetComponent<Rigidbody> ().isKinematic = true;

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
