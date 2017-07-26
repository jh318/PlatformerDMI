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
		if(Input.GetKeyDown(KeyCode.G)){
			GameManager.instance.PlayerIsMechaman = false;
			GameManager.instance.PlayerIsZero = false;
			GameManager.instance.PlayerIsDemonRobot = true;
			GameManager.instance.GetComponent<SceneLoader>().LoadScene(sceneToLoad);
		}
		if(Input.GetKeyDown(KeyCode.Alpha0)){
			GameManager.instance.PlayerIsMechaman = false;
			GameManager.instance.PlayerIsDemonRobot = false;
			GameManager.instance.PlayerIsZero = true;
			GameManager.instance.GetComponent<SceneLoader>().LoadScene(sceneToLoad);
		}
		if(Input.GetKeyDown(KeyCode.Z)){
			GameManager.instance.PlayerIsMechaman = true;
			GameManager.instance.PlayerIsDemonRobot = false;
			GameManager.instance.PlayerIsZero = false;
			GameManager.instance.GetComponent<SceneLoader>().LoadScene(sceneToLoad);
		}
	}
}
