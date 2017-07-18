using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

	public string roomToLoad;

	void OnTriggerStay2D(Collider2D other)
	{
		if(Input.GetAxisRaw ("Vertical") < -0.5f){
			LoadRoom(roomToLoad);
		}
	}

	void LoadRoom(string roomName){
		SceneManager.LoadScene(roomName);
	}
}
