using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	void OnDestroy () {
		//Debug.Log("FUCK");
	}

	void Update(){
	}

	void DeathCheck(){
		if(PlayerController.instance.gameObject.activeSelf == false){
			//GetComponent<SceneLoader>().LoadScene("MainMenu");
		}
	}

	public static void LoadLevelPosition(string sceneName, Vector3 newPosition){
		instance.StartCoroutine(instance.LoadLevelPositionCoroutine(sceneName, newPosition));
	}

	public IEnumerator LoadLevelPositionCoroutine(string sceneName, Vector3 newPosition){
		yield return SceneManager.LoadSceneAsync(sceneName);
		PlayerController.instance.transform.position = newPosition;
	}
}
