using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDispatcher : MonoBehaviour {
	public GameObject dialog;
	public float wait = 1f;

	bool eventTriggered;
	public bool cantTrigger { get; set; }

	void OnTriggerEnter(Collider other) {
		if (!cantTrigger) {
			if (other.transform.root.CompareTag ("Player") && !eventTriggered) {
				eventTriggered = true;
				StartCoroutine (DispatchDialog ());
			}
		}
	}

	IEnumerator DispatchDialog () {
		yield return new WaitForSecondsRealtime (wait);
		dialog.GetComponent<Dialog> ().StartDialog ();
		Destroy (this.gameObject);
	}
}
