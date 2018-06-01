using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCube : MonoBehaviour {
	public GameObject brokenCube;
	GameObject instantiatedObject;
	public float timeWait;

	void Start() {
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Ground"), LayerMask.NameToLayer ("BrokenGround"));
	}

	void OnParticleCollision(GameObject ps) {
		instantiatedObject = Instantiate(brokenCube, 
					new Vector3 (transform.position.x, transform.position.y, transform.position.z + 5.0f), transform.rotation);
		
		Vector3 newPos = transform.position;
		newPos.y -= 120f;
		transform.position = newPos;
		StartCoroutine (RespawnCube ());

		//Destroy Sphere collider
		Destroy (instantiatedObject.transform.GetChild (instantiatedObject.transform.childCount - 1).gameObject, 0.4f);
		Destroy (instantiatedObject, 4f);
	}

	IEnumerator RespawnCube () {
		yield return new WaitForSeconds (timeWait);
		Vector3 newPos = transform.position;
		newPos.y += 120f;
		transform.position = newPos;
	}
}
