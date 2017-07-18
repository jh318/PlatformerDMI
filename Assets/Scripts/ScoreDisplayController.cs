using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayController : MonoBehaviour {

	public Text styleText;
	public Text airTimeText;

	void Start(){
		styleText.text = "Ranking: " + ComboTextController.instance.HighestRating.ToString();
		airTimeText.text = "Air Time: " + ComboTextController.instance.LongestAirTime.ToString("F3") + " Seconds";
		DisablePlayerUI();
	}

	void DisablePlayerUI(){
		UIManager.ShowHide(false);
	}
}
