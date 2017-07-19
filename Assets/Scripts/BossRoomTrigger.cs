using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour {

	public GameObject bossWall;

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.GetComponent<PlayerController>()) 	bossWall.SetActive(true);
	}
}
