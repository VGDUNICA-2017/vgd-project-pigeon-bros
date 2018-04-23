using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTrap : MonoBehaviour {
	Animation anim;
	DelayAnimation scriptAnim;

	// Use this for initialization
	void Start () {
		scriptAnim = GetComponent<DelayAnimation> ();

		anim = GetComponent<Animation>();
		anim["Up Down"].wrapMode = WrapMode.Once;
		StartCoroutine ("StartAnimation");
	}

	IEnumerator StartAnimation() {
		yield return new WaitForSeconds (1.2f);
		anim.Play ();
		scriptAnim.enabled = true;
	}
}
