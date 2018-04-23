using UnityEngine;
using System.Collections;

public class DelayAnimation : MonoBehaviour
{
	Animation anim;
	bool delayAnim = true;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
		anim["Up Down"].wrapMode = WrapMode.Once;
	}

	void Update () {
		if (delayAnim) {
			delayAnim = false;
			StartCoroutine ("LoopDelayAnimation");
		}
	}

	IEnumerator LoopDelayAnimation() {
		anim.Play ();
		yield return new WaitForSeconds (.9f);
		delayAnim = true;
	}
}

