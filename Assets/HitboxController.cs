using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour {

	public float launchForce = 50.0f;

	GameObject parent;
	bool enemyHit = false;
	public bool EnemyHit{
		get { return enemyHit; }
		set {enemyHit = value;  }
	}
	GameObject enemyObject;
	public GameObject EnemyObject{
		get {return enemyObject; }
		set {enemyObject = value; }
	}
	

	void Start(){
		GetComponentInParent<PlayerController>();
	}

	public void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.tag == "Enemy"){
			enemyObject = c.gameObject;
			enemyHit = true;
		}
	}


}
