using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoxController : MonoBehaviour {

	public string resetPoint = "";

	void OnTriggerEnter2D(Collider2D c){
		c.gameObject.SetActive(false);
		if(c.gameObject.CompareTag("Player")){
			SceneLoader.instance.LoadScene(resetPoint);
		}
	}
}
