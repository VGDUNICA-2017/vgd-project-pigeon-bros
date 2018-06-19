using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeResolution : MonoBehaviour {
	Resolution currentResolution;

	float orthoSize;
	float orthoSizeRatio;

	const float screenRatioOnDevelopment = 1024f / 512f;

	/*
	 * 1024 X 512 => orthographicSize = 9
	 * 1024 / 512 = 2
	 * Example other resolution
	 * 1366 * 768
	 * 1366 / 768 = 1.7786
	 * newOrthographicSize = orthographicSize * (2 / 1.7786)
	 * newOrthographicSize = orthographicSize * ((1024 / 512) / (1366 / 768))
	 */

	// Use this for initialization
	void Awake () {
		orthoSize = Camera.main.orthographicSize;
		currentResolution = Screen.currentResolution;

		orthoSizeRatio = screenRatioOnDevelopment / ((float)currentResolution.width / (float)currentResolution.height);
		orthoSize = orthoSize * orthoSizeRatio;

		Camera.main.orthographicSize = orthoSize;
	}
	
	public float GetOrthoSize() {
		return this.orthoSize;
	}
}
