using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextController : MonoBehaviour {

	public static ComboTextController instance;

	float airTimer = 0.0f;
	public Text comboText;

	static float longestAirTime = 0.0f;
	string highestRating = "Default";

	public float LongestAirTime{
		get { return longestAirTime; }
	}
	public string HighestRating{
		get{ return highestRating; }
	}

	void Awake(){
		if(instance == null){
			instance = this;
		} else{
		//	Destroy(gameObject);
		}
	}

	void Start () {
		//comboText = GetComponent<Text>();
		highestRating = "Grounded!!!";
	}
	
	void Update () {
		if(PlayerController.instance == null){
			return;
		}
		if(!PlayerController.instance.isGrounded){
			airTimer += Time.deltaTime;
			//comboText.text = airTimer.ToString("F3");
		}
		if(PlayerController.instance.isGrounded){
			airTimer = 0.0f;
			comboText.text = "Grounded!!!";
			//comboText.text = airTimer.ToString("F3");
		}
		ComboCommentary();
		Ranker();
		//Debug.Log(highestRating);
	}

	void ComboCommentary(){
		if(airTimer > 5.0f) comboText.text = "Dood!";
		if(airTimer > 10.0f) comboText.text = "Crazy!";
		if(airTimer > 15.0f) comboText.text = "Barrel Rollin'!";
		if(airTimer > 20.0f) comboText.text = "ACE";
		if(airTimer > 25.0f) comboText.text = "SUPA'!";
		if(airTimer > 30.0f) comboText.text = "SUPA' FLYYYY!";
		if(airTimer > 35.0f) comboText.text = "SUPA' FLY STYLIN' ";
	}

	void Ranker(){
		if(airTimer > longestAirTime) longestAirTime = airTimer;
		if(longestAirTime >= 5.0f) highestRating = "Dood!";
		if(longestAirTime >= 10.0f) highestRating = "Crazy!";
		if(longestAirTime >= 15.0f) highestRating = "Barrel Rollin'!";
		if(longestAirTime >= 20.0f) highestRating = "ACE";
		if(longestAirTime >= 25.0f) highestRating = "SUPA'!";
		if(longestAirTime >= 30.0f) highestRating = "SUPA' FLYYYY!";
		if(longestAirTime >= 35.0f) highestRating = "SUPA' FLY STYLIN'";
	}
}
