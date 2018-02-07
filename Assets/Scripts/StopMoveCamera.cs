using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMoveCamera : MonoBehaviour {

	void Update () {
		if (transform.position.x > 553.5f)
			//Disattivare spostamento camera sul personaggio per non uscire fuori mappa
			;
	}
}
