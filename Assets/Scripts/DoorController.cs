using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

	public string roomToLoad;
	public Vector3 levelPosition = new Vector3(0,0,0);
	bool startedLoad;

	void OnTriggerStay2D(Collider2D other)
	{
		if(!startedLoad && Input.GetAxisRaw ("Vertical") < -0.5f){
			LoadRoom();
		}
	}

	void LoadRoom(){
		startedLoad = true;
		GameManager.LoadLevelPosition(roomToLoad, levelPosition);
	}
}
