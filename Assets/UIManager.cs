using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public static UIManager instance;
	public GameObject healthBar;
	public GameObject comboText;
	public GameObject textBox;

	void Awake(){
		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}
	}

	// void Start(){
	// 	healthBar.SetActive(false);
	// 		comboText.SetActive(false);
	// 		textBox.SetActive(false);
	// }

	void OnEnable () {
		SceneManager.activeSceneChanged += SceneChanged;
	}

	void OnDisable () {
		SceneManager.activeSceneChanged -= SceneChanged;
	}

	void SceneChanged (Scene fromScene, Scene toScene) {
		if (toScene.name != "MainMenu" || toScene.name !=  "ScoreScreen") {
			// activate stuff
			healthBar.SetActive(true);
			comboText.SetActive(true);
			//textBox.SetActive(true);
		}else{
			healthBar.SetActive(false);
			comboText.SetActive(false);
			textBox.SetActive(false);
		}
		if(toScene.name == "RedStage"){
			healthBar.SetActive(true);
			comboText.SetActive(true);
		}
	
	}
}
