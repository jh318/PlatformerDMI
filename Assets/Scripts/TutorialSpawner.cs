using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour {

	public GameObject spawnObject;
	public GameObject deathCheck;

	void Update(){
		if(!deathCheck.activeSelf){
			StartCoroutine("Respawn");
		}
	}
	IEnumerator Respawn(){
		yield return new WaitForEndOfFrame();
		GameObject spawnedObject = Instantiate(spawnObject, transform.position, Quaternion.identity);
		deathCheck = spawnedObject;
		yield return new WaitForEndOfFrame();
	}
}
