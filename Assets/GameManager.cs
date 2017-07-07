using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Awake(){
		if(instance == null){
			instance = this;
		}
	}

	void Update(){
	}

	void DeathCheck(){
		Debug.Log("Update");
		if(PlayerController.instance.gameObject.activeSelf == false){
			Debug.Log("Here");
			GetComponent<SceneLoader>().LoadScene("MainMenu");
		}
	}
}
