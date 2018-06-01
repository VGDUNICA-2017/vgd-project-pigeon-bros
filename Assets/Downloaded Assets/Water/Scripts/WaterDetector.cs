using UnityEngine;
using System.Collections;

public class WaterDetector : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D hit)
	{
		if (hit.transform.root.CompareTag ("Player")) {
			if (hit.GetComponent<Rigidbody2D> () != null) {
				transform.parent.GetComponent<Water> ().Splash (transform.position.x, 30f * hit.GetComponent<Rigidbody2D> ().mass / 40f);
				hit.transform.root.GetComponent<Thirang> ().FatalDamage ();
			}
		}
	}
}