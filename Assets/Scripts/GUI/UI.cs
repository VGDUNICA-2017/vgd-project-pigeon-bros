using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	Thirang thirang;

	public GameObject hp;
	Text hpText;

	public GameObject mp;
	Text mpText;

	public GameObject hpPotions;
	Text hpPotionsText;

	public GameObject mpPotions;
	Text mpPotionsText;

	public GameObject gold;
	Text goldText;

	public GameObject exp;
	Text expText;

	// Use this for initialization
	void Start () {
		thirang = FindObjectOfType<Thirang> ();

		hpText = hp.GetComponent<Text> ();
		mpText = mp.GetComponent<Text> ();
		hpPotionsText = hpPotions.GetComponent<Text> ();
		mpPotionsText = mpPotions.GetComponent<Text> ();
		goldText = gold.GetComponent<Text> ();
		expText = exp.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		hpText.text = thirang.health.ToString ();
		mpText.text = thirang.mana.ToString ();
		hpPotionsText.text = thirang.hpPotions.ToString ();
		mpPotionsText.text = thirang.mpPotions.ToString ();
		goldText.text = thirang.gold.ToString ();
		expText.text = thirang.exp.ToString ();
	}
}
