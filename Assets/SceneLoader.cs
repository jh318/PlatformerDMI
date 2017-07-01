using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public string restartScene;
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			LoadScene(restartScene);
		}
	}

	public void LoadScene(string scene){
		SceneManager.LoadScene(scene);
	}
}
