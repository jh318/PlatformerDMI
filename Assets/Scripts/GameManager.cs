using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public bool playerIsMechaman = false;
	bool playerIsDemonRobot = false;
	bool playerIsZero = false;

	public bool PlayerIsMechaman{
		get { return playerIsMechaman; }
		set { playerIsMechaman = value;}
	}

	public bool PlayerIsDemonRobot{
		get { return playerIsDemonRobot; }
		set { playerIsDemonRobot = value; }
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
		
		//Debug.Log("Level Loaded");
		//Debug.Log(scene.name);
		//Debug.Log(mode);
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
		if(PlayerController.instance.gameObject.active)
		PlayerController.instance.transform.position = newPosition;
	}

	void EnablePlayerCharacter(){
		if(playerIsZero){
			GameObject playerObject = Spawner.Spawn("Zero");
			playerObject.transform.position = FindObjectOfType<CharacterSpawnPoint>().transform.position;
		}
		if(playerIsDemonRobot){
			GameObject playerObject = Spawner.Spawn("RobotMan");
			playerObject.transform.position = FindObjectOfType<CharacterSpawnPoint>().transform.position;
		}
		if(playerIsMechaman){
			GameObject playerObject = Spawner.Spawn("MechaMan");
			playerObject.transform.position = FindObjectOfType<CharacterSpawnPoint>().transform.position;
		}
	}
}
