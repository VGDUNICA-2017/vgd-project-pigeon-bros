using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void NewGame() {
		Thirang.NewGame ();
		StartCoroutine (LoadScene ());
	}

	public void Load() {
		if (PlayerPrefs.HasKey ("Scene"))
			SceneManager.LoadScene (PlayerPrefs.GetString ("Scene"));
	}

	public void Quit() {
		Application.Quit();
		Debug.Log("Application.Quit() non fa niente nell'editor");
	}

	IEnumerator LoadScene() {
		AsyncOperation sceneLoad = SceneManager.LoadSceneAsync ("LivelloCielo");

		while (!sceneLoad.isDone) {
			yield return null;

		}
	}
}
