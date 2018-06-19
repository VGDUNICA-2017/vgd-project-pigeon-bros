using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {
	public static bool dialogActive;

	public GameObject dialogGUI;

	public GameObject dialogText;
	public GameObject dialogName;
	public GameObject thirangFrame;
	public GameObject fairyFrame;

	bool eventTriggered;

	public enum Dialogs
	{
		skyDialog, earthDialog, seaDialog, fireDialog, merchantDialog, rockDialog, superJumpDialog, finalDialog
	}

	public Dialogs currentDialog;
	private KeyValuePair<string, string>[] _currentDialog;
	private int currentIndex;

	KeyValuePair<string, string>[] skyDialog = new KeyValuePair<string, string>[] {
		new KeyValuePair<string,string> ("thirang", "Where am I?"),
		new KeyValuePair<string,string> ("fairy", "Thirang, you're finally here! The Reign of Helensia is in very danger! YOU MUST HELP US"),
		new KeyValuePair<string,string> ("thirang", "Thirang? Helensia? What are you talking about?"),
		new KeyValuePair<string,string> ("fairy", "I HAVE NO TIME TO EXPLAIN, PLEASE JUST GO AHEAD"), 
		new KeyValuePair<string,string> ("fairy", "Use A and D to move, SPACE to jump, LEFT CLICK to attack and CLICK again for SWING ATTACK, " +
			"RIGHT CLICK to shield, hold LEFT SHIFT to run."), 
		new KeyValuePair<string,string> ("fairy", "Use 1 to activate Berserk and enhance your attacks, " +
			"Z to use health potions and X to use mana potions."),
		new KeyValuePair<string,string> ("fairy", "If you need a pause just use ESC"),
		new KeyValuePair<string,string> ("fairy", "Now stop wasting time, GO GO GO!")
	};

	KeyValuePair<string, string>[] earthDialog = new KeyValuePair<string, string>[] {
		new KeyValuePair<string,string> ("fairy", "Good job, Thirang! You defeated the Mage and the power of Sky is back!"),
		new KeyValuePair<string,string> ("thirang", "Are you ever going to tell me anything about this world?"),
		new KeyValuePair<string,string> ("fairy", "Sorry, I am Glittercactus, one of the many fairies of this world. Unfortunately I'm the only one with any power left."),
		new KeyValuePair<string,string> ("fairy", "This is the Reign of Helensia, a world that only the pure hearted souls can reach. It's a peaceful land " +
			"where people can rest and enjoy the mystical power of this world. But now a evil being came and stole " +
			"the power of Sky, Earth, Sea and Fire. Only you can save us"),
		new KeyValuePair<string,string> ("fairy", "Now you can use 2 to attack with Cyclone Spin and hit all the enemies around you."),
		new KeyValuePair<string,string> ("fairy", "Good luck Thirang")
	};

	KeyValuePair<string, string>[] seaDialog = new KeyValuePair<string, string>[] {
		new KeyValuePair<string,string> ("fairy", "Good job, Thirang! You defeated the Demon and the power of Earth is back!"),
		new KeyValuePair<string,string> ("thirang", "Why was I summoned in this world? Isn't there anyone else?"),
		new KeyValuePair<string,string> ("fairy", "The purest heart in the Earth is our best warrior. In this world there's no thing as combat skill: " +
			"the purest the heart, the strongest the warrior. You have the purest heart in the Earth, " +
			"you are our best bet to defeat this evil monster"),
		new KeyValuePair<string,string> ("fairy", "...")
	};

	KeyValuePair<string, string>[] fireDialog = new KeyValuePair<string,string>[] {
		new KeyValuePair<string,string> ("fairy", "Good job, Thirang! You defeated the Giant Mutant and the power of Sea is back!"),
		new KeyValuePair<string,string> ("fairy", "This is the last step before the final fight. There, the Lava Monster will wait for you. " +
			"Defeating him will bring back the peace in the Reign of Helensia, and all of its inhabitants will be free."),
		new KeyValuePair<string,string> ("fairy", "Right now you need to find 4 power ups disseminated in this level. Once you have got all the 4 power ups, " +
			"you will be able to access to the final fight."),
		new KeyValuePair<string,string> ("fairy", "The Reign of Helensia has faith in you!"),
		new KeyValuePair<string,string> ("thirang", "I won't let you all down.")
	};

	KeyValuePair<string, string>[] merchantDialog = new KeyValuePair<string,string>[] {
		new KeyValuePair<string,string> ("fairy", "This is the merchant: strange guy, doesn't talk but he has a lot of good stuff to sell"),
		new KeyValuePair<string,string> ("fairy", "Get close to him and press M to buy.") 
	};

	KeyValuePair<string, string>[] rockDialog = new KeyValuePair<string,string>[] {
		new KeyValuePair<string,string> ("fairy", "This rock can be destoyed once you will reach level 2. You need to earn 5000 XP for level 2, and 10000 XP for level 3. " +
			"Level 3 will give you the \"SuperJump\" ability. As soon as you will be in the right situation press F to use the ability.")
	};

	KeyValuePair<string, string>[] superJumpDialog = new KeyValuePair<string,string>[] { 
		new KeyValuePair<string,string> ("fairy", "This is the \"SuperJump\" event. If you reached level 3 press F to use the ability.")
	};

	KeyValuePair<string, string>[] finalDialog = new KeyValuePair<string,string>[] { 
		new KeyValuePair<string,string> ("fairy", "You did it, you saved it! You saved Helensia! Thank you Thirang!"),
		new KeyValuePair<string,string> ("thirang", "What will happen now?"),
		new KeyValuePair<string,string> ("fairy", "You will return to where you came from, it's over. Thank you again")
	};

	void Start() {
		switch (currentDialog) {
			case Dialogs.skyDialog:
				_currentDialog = skyDialog;
				break;
			case Dialogs.earthDialog:
				_currentDialog = earthDialog;
				break;
			case Dialogs.seaDialog:
				_currentDialog = seaDialog;
				break;
			case Dialogs.fireDialog:
				_currentDialog = fireDialog;
				break;
			case Dialogs.merchantDialog:
				_currentDialog = merchantDialog;
				break;
			case Dialogs.rockDialog:
				_currentDialog = rockDialog;
				break;
			case Dialogs.superJumpDialog:
				_currentDialog = superJumpDialog;
				break;
			case Dialogs.finalDialog:
				_currentDialog = finalDialog;
				break;
			default:
				break;
		}
	}

	public void StartDialog() {
		Pause ();
		dialogGUI.SetActive (true);
		NextDialog ();
		dialogActive = true;
	}

	public void NextDialog() {
		if (currentIndex < _currentDialog.Length) {
			ResolveSpeaker (_currentDialog [currentIndex].Key);
			dialogText.GetComponent<Text> ().text = _currentDialog [currentIndex].Value;
			currentIndex++;
		} else {
			Resume ();
			dialogActive = false;
			if (currentDialog == Dialogs.finalDialog) {
				this.gameObject.AddComponent<TeleportBehaviour> ().FinalBoss ("Menu");	
			}

			Destroy (this.gameObject);
		}
	}

	void Pause() {
		Time.timeScale = 0f;
	}

 	void Resume() {
		Time.timeScale = 1f;
		dialogGUI.SetActive (false);
	}

	void ResolveSpeaker (string speaker) {
		thirangFrame.SetActive (false);
		fairyFrame.SetActive (false);

		switch (speaker) {
			case "thirang":
				thirangFrame.SetActive (true);
				dialogName.GetComponent<Text> ().text = "Thirang";
				break;
			case "fairy":
				fairyFrame.SetActive (true);
				dialogName.GetComponent<Text> ().text = "Glittercactus";
				break;
		}
	}
}
