using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpecialProperties : MonoBehaviour {
	
	//Common Components
	Rigidbody2D body;
	
	//Common Variables
	bool hitStun = false;
	float gravity;

	void Start(){
		body.GetComponent<Rigidbody2D>();
		gravity = body.gravityScale;
	}
	
	//Special Properties
	public IEnumerator SetHitStun(float duration){
		hitStun = true;
		yield return new WaitForSeconds(duration);
		hitStun = false;
	}

	public IEnumerator FreezePosition(){
		while(hitStun){
			body.velocity = new Vector2(0,0);
			body.gravityScale = 0;
			yield return new WaitForEndOfFrame();
		}
		body.gravityScale = gravity;
	}

	public IEnumerator SetKnockBack(float xVelocity, float yVelocity, float duration){
		
		yield return new WaitForEndOfFrame();
	}
}
