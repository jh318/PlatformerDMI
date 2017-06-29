using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	Rigidbody2D body;

	float gravity;

	void Start(){
		body = GetComponent<Rigidbody2D>();
		gravity = body.gravityScale;
		body.velocity = new Vector2(1,0);
	}

	void Update(){
		//body.velocity = new Vector3(1,body.velocity.y,0);
	}
}
