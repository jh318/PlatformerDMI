using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	bool playerIsDemonRobot = true;
	bool playerIsZero = true;

	public bool PlayerIsDemonRobot{
		get{ return playerIsDemonRobot; }
		set {playerIsDemonRobot = value;}
	}
	
	public bool PlayerIsZero{
		get { return playerIsZero; }
		set { playerIsZero = value; }
	}

	void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
		Debug.Log("EnabledGM");
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		if(scene.name != "MainMenu" && scene.name != "ScoreScreen"){
			EnablePlayerCharacter();
		}
		
		Debug.Log("Level Loaded");
		Debug.Log(scene.name);
		Debug.Log(mode);
	}

	void OnDestroy () {
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

	void EnablePlayerCharacter(){
		if(playerIsDemonRobot && !playerIsZero){
			Zero playerZero = FindObjectOfType<Zero>();
			playerZero.GetComponent<PlayerController>().enabled = false;
			playerZero.gameObject.SetActive(false);
			DemonRobot playerRobot = FindObjectOfType<DemonRobot>();
			playerRobot.gameObject.SetActive(true);
			playerRobot.GetComponent<PlayerController>().enabled = true;

		}
		if(playerIsZero && !playerIsDemonRobot){
			DemonRobot playerRobot = FindObjectOfType<DemonRobot>();
			playerRobot.GetComponent<PlayerController>().enabled = false;
			playerRobot.gameObject.SetActive(false);
			Zero playerZero = FindObjectOfType<Zero>();
			playerZero.gameObject.SetActive(true);
			playerZero.GetComponent<PlayerController>().enabled = true;

		}
	}
}
