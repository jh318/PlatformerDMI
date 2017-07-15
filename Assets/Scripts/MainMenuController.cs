﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour {

	public string sceneToLoad;


	void Start(){
		SceneManager.LoadScene("GameManager", LoadSceneMode.Additive);
		if(UIManager.instance.enabled){
			UIManager.instance.gameObject.SetActive(false);
		}
	}
	void Update(){
		if(Input.GetButtonDown("SlashButton")){
			GameManager.instance.GetComponent<SceneLoader>().LoadScene(sceneToLoad);
		}
	}
}