using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	public string sceneToLoad;

	void Update(){
		if(Input.GetButtonDown("SlashButton")){
			GameManager.instance.GetComponent<SceneLoader>().LoadScene(sceneToLoad);
		}
	}
}
