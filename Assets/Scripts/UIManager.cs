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
			//DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
		}
	}

	void OnEnable () {
		SceneManager.activeSceneChanged += SceneChanged;
	}

	void OnDisable () {
		SceneManager.activeSceneChanged -= SceneChanged;
	}

	void SceneChanged (Scene fromScene, Scene toScene) {
		ShowHide (toScene.name != "MainMenu" && toScene.name !=  "ScoreScreen");
	}

	public static void ShowHide (bool show) {
		if (show) {
			instance.healthBar.SetActive(true);
			instance.comboText.SetActive(true);
		}
		else {
			instance.healthBar.SetActive(false);
			instance.comboText.SetActive(false);
			instance.textBox.SetActive(false);
		}
	}
}