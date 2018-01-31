using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_and_CamMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MoveCameraLvl01Boss scr = GetComponent<MoveCameraLvl01Boss> ();
	
	
	/*--Start boss animation--*/

	/*--End boss animation--*/
		scr.enabled = true;
	}
}
