using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false;
	public GameObject PauseMenuUI;
	public GameObject SavedButton;
	public GameObject UI;

	private string defaultSavedText;

	void Update () {
		if (Input.GetButtonDown ("Pause")) {
			if (!Dialog.dialogActive && !MerchantBehaviour.onStore) {
				if (GameIsPaused) {
					if (SavedButton.activeSelf) {
						SavedButton.SetActive (false);
						SavedButton.GetComponentInChildren<Text> ().text = defaultSavedText;
					}
					Resume ();
				} else {
					Pause ();
				}
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			if (SavedButton.activeSelf) {
				SavedButton.SetActive (false);
				SavedButton.GetComponentInChildren<Text> ().text = defaultSavedText;
				PauseMenuUI.SetActive (true);
			}
		}
	}

	public void Resume() {
		UI.SetActive (true);
		PauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	void Pause() {
		UI.SetActive (false);
		PauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	public void Save() {
		Thirang thirang = FindObjectOfType<Thirang> ();
		Text _text = SavedButton.GetComponentInChildren<Text> ();
		defaultSavedText = _text.text;

		KeyValuePair<bool, string> canSaveResponse = thirang.CanSave ();

		if (canSaveResponse.Key) {
			thirang.SaveThirangData (true);
			PauseMenuUI.SetActive (false);
			SavedButton.SetActive (true);
		} else {
			_text.text = canSaveResponse.Value;
			PauseMenuUI.SetActive (false);
			SavedButton.SetActive (true);
		}
	}

	public void Quit() {
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Menu");
	}
}
