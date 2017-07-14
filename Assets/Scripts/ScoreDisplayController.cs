using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayController : MonoBehaviour {

	public Text styleText;
	public Text airTimeText;

	void Start(){
		styleText.text = ComboTextController.instance.HighestRating.ToString();
		airTimeText.text = ComboTextController.instance.LongestAirTime.ToString("F3");
	}
}
