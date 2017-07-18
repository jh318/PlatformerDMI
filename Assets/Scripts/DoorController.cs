using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

	public string roomToLoad;
	public Vector3 levelPosition = new Vector3(0,0,0);

	void OnTriggerStay2D(Collider2D other)
	{
		if(Input.GetAxisRaw ("Vertical") < -0.5f){
			LoadRoom();
		}
	}

	void LoadRoom(){
		GameManager.LoadLevelPosition(roomToLoad, levelPosition);
	}
}
