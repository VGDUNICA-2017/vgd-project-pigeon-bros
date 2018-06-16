using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestruction : MonoBehaviour {
	public GameObject rockDestroyed;

	public void DestroyRock() {
		Vector3 pos = transform.position;
		pos.y += 8f;
		Instantiate (rockDestroyed, pos, Quaternion.identity);
		Destroy (this.gameObject);
	}
}
