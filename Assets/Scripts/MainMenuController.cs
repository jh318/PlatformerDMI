using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour {

	public string sceneToLoad;


	void Start(){
		//SceneManager.LoadScene("GameManager", LoadSceneMode.Additive);
	}
	void Update(){
		if(Input.GetButtonDown("SlashButton")){
			GameManager.instance.PlayerIsZero = false;
			GameManager.instance.PlayerIsDemonRobot = true;
			GameManager.instance.GetComponent<SceneLoader>().LoadScene(sceneToLoad);
		}
		if(Input.GetKeyDown(KeyCode.Alpha0)){
			GameManager.instance.PlayerIsDemonRobot = false;
			GameManager.instance.PlayerIsZero = true;
			GameManager.instance.GetComponent<SceneLoader>().LoadScene(sceneToLoad);
		}
	}
}
