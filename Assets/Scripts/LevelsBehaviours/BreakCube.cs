using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCube : MonoBehaviour {
	public GameObject brokenCube;
	GameObject instantiatedObject;
	public float timeWait;
	bool startTimer;
	float timer;

	void Start() {
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Ground"), LayerMask.NameToLayer ("BrokenGround"));
	}

	void Update() {
		if (startTimer) {
			timer += Time.deltaTime;
		}

		if (timer >= timeWait) {
			Vector3 newPos = transform.position;
			newPos.y += 120f;
			transform.position = newPos;

			timer = 0f;
			startTimer = false;
		}
	}

	void OnParticleCollision(GameObject ps) {
		instantiatedObject = (GameObject)Instantiate(
			brokenCube, 
				new Vector3 (transform.position.x, transform.position.y, transform.position.z + 5.0f),
					transform.rotation);
		
		Vector3 newPos = transform.position;
		newPos.y -= 120f;
		transform.position = newPos;
		startTimer = true;
		Destroy (instantiatedObject.transform.GetChild (instantiatedObject.transform.childCount - 1).gameObject, 0.4f);
		Destroy (instantiatedObject, 4f);
	}
}
