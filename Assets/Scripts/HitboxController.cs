using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour {

	GameObject parent;
	GameObject enemyObject;
	SpecialProperties enemyProperties; //Requires script on enemy
	SpecialProperties parentProperties; //Requires script on root
	List<GameObject> targets = new List<GameObject>();
	bool enemyHit = false;

	public bool EnemyHit{
		get { return enemyHit; }
		set {enemyHit = value;  }
	}
	public GameObject EnemyObject{
		get {
			if(enemyObject != null)
				return enemyObject;
			else
				return null;
		}
		set {enemyObject = value; }
	}

	public SpecialProperties ParentProperties{
		get {return parentProperties;}
	}
	
	void Start(){
		GetComponentInParent<PlayerController>();
		parentProperties = GetComponentInParent<SpecialProperties>();
	}

	// public void OnTriggerEnter2D(Collider2D c){

	// 	targets.Add(c.gameObject);

	// 	if(c.gameObject.GetComponentInParent<SpecialProperties>()){
	// 		Debug.Log("Enemy with SProperties Hit");
	// 		//enemyProperties = c.gameObject.GetComponentInParent<SpecialProperties>();
	// 		enemyObject = c.gameObject;
	// 		enemyHit = true;
	// 		parentProperties.Target = c.gameObject;
	// 		//parentProperties.EnemyObject(enemyObject);
	// 	}
	// }

	public void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.GetComponent<SpecialProperties>()) {
			targets.Add(c.gameObject);
		}

		if(targets.Count == 0) Debug.Log("Enemy miss");

		parentProperties.Targets = targets;
	}

	void OnDisable(){
		targets.Clear();
		parentProperties.Targets.Clear();
	}
}
