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
	float launchHeight = 3;
	List<GameObject> targets = new List<GameObject>();

	public List<GameObject> Targets{
		get{return targets;}
		set{targets = value;}
	}

	void Start(){
		body = GetComponent<Rigidbody2D>();
		gravity = body.gravityScale;
		launchHeight = 3;
		
	}

	void Update(){
	}

	//Common Calculations
	public float LaunchUnitHeight(float unitHeight){
		return Mathf.Sqrt(2 * unitHeight * Mathf.Abs(Physics2D.gravity.y) * body.gravityScale);
	}

	//Special Properties
	public IEnumerator SetHitStun(float duration, bool hitStunVelocity = true){
		hitStun = true; 
		if(duration > 0 && hitStunVelocity)
			StartCoroutine(HitStunVelocity());
		yield return new WaitForSeconds(duration);
		hitStun = false;
	}

	public IEnumerator HitStunVelocity(){
		body.gravityScale = 0;
		while(hitStun){
			body.velocity = new Vector2(0,0);
			yield return new WaitForEndOfFrame();
		}
		body.gravityScale = gravity;
	}

	public IEnumerator Launcher(){
			StartCoroutine(SetHitStun(1.0f, false));
		yield return new WaitForEndOfFrame();
		body.velocity = new Vector2(0, LaunchUnitHeight(launchHeight));
		while(hitStun){
			if(body.velocity.y < 0.1f){
				body.gravityScale = 0;
				yield return new WaitForEndOfFrame();
			}
		yield return new WaitForEndOfFrame();
		}
		body.gravityScale = gravity;
			
	}

	public void Launch(){
		body.velocity = new Vector2(0, LaunchUnitHeight(launchHeight));	
	}

	public IEnumerator SetKnockBack(float xVelocity, float yVelocity){
		body.velocity = new Vector3(xVelocity, yVelocity);


		yield return new WaitForEndOfFrame();
	}

	public void ReflectProjectile(){
		if(gameObject.layer == 11){
			body.velocity = new Vector2(body.velocity.x*-1, body.velocity.y);
			gameObject.layer = 10;
			PlayerController.instance.JumpCount--;
		}
	}
}
