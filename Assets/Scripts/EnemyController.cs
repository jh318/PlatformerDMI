using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	Rigidbody2D body;

	float gravity;
	bool hitStun = false;

	void Start(){
		body = GetComponent<Rigidbody2D>();
		gravity = body.gravityScale;
	}

	void Update(){
		body.velocity = new Vector3(1,0,0);
	}

	//Collision
	void OnTriggerEnter2D(Collider2D c){
		if(hitStun){
			StartCoroutine("FreezePosition");
		}
	}

	//Special Properties
	public IEnumerator FreezePosition(){
			while(hitStun){
				body.velocity = new Vector2(0,0);
				body.gravityScale = 0;
				yield return new WaitForEndOfFrame();
			}
			body.gravityScale = gravity;
		}

	public IEnumerator SetHitStun(float duration){
		hitStun = true;
		yield return new WaitForSeconds(duration);
		hitStun = false;
	}

	public IEnumerator SetKnockBack(float distance){
		yield return new WaitForEndOfFrame();
	}
}
