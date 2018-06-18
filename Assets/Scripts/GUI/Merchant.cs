using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour {
	Thirang thirang;

	public GameObject thirangGold;
	Text thirangGoldText;

	public GameObject hp;
	int hpCost;

	public GameObject mp;
	int mpCost;

	public GameObject hpPotions;
	int hpPotionsCost;

	public GameObject mpPotions;
	int mpPotionsCost;

	public GameObject atkUp;
	int atkUpCost;

	public GameObject defUp;
	int defUpCost;

	// Use this for initialization
	void Start () {
		thirang = FindObjectOfType<Thirang> ();

		thirangGoldText = thirangGold.GetComponent<Text> ();

		hpCost = int.Parse (hp.GetComponent<Text> ().text);
		mpCost = int.Parse (mp.GetComponent<Text> ().text);
		hpPotionsCost = int.Parse (hpPotions.GetComponent<Text> ().text);
		mpPotionsCost = int.Parse (mpPotions.GetComponent<Text> ().text);
		atkUpCost = int.Parse (atkUp.GetComponent<Text> ().text);
		defUpCost = int.Parse (defUp.GetComponent<Text> ().text);

		thirangGoldText.text = thirang.gold.ToString();
	}

	public void AtkUp() {
		if (thirang.gold >= atkUpCost) {
			thirang.gold -= atkUpCost;
			thirangGoldText.text = thirang.gold.ToString ();
			thirang.BuyObject ("atkUp", 10);
		}
	}

	public void DefUp() {
		if (thirang.gold >= defUpCost) {
			thirang.gold -= defUpCost;
			thirangGoldText.text = thirang.gold.ToString ();
			thirang.BuyObject ("defUp", 10);
		}
	}

	public void hpUp() {
		if (thirang.gold >= hpCost) {
			thirang.gold -= hpCost;
			thirangGoldText.text = thirang.gold.ToString ();
			thirang.BuyObject ("hp", 2000);
		}
	}

	public void mpUp() {
		if (thirang.gold >= mpCost) {
			thirang.gold -= mpCost;
			thirangGoldText.text = thirang.gold.ToString ();
			thirang.BuyObject ("mp", 50);
		}
	}

	public void hpPotionUp() {
		if (thirang.gold >= hpPotionsCost) {
			thirang.gold -= hpPotionsCost;
			print (thirang.gold + " " + thirangGoldText.text);
			thirangGoldText.text = thirang.gold.ToString ();
			thirang.BuyObject ("hpPotion", 1);
		}
	}

	public void mpPotionUp() {
		if (thirang.gold >= mpPotionsCost) {
			thirang.gold -= mpPotionsCost;
			thirangGoldText.text = thirang.gold.ToString ();
			thirang.BuyObject ("mpPotion", 1);
		}
	}
}
