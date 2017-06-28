using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpecialProperties : MonoBehaviour {
	
	//Common Components
	Rigidbody2D body;
	// HitboxController hitbox; //Need a box trigger on a child object with this script
	
	//Common Variables
	bool hitStun = false;
	float gravity;
	float launchHeight;
	GameObject target;
	Rigidbody2D targetBody;


	//Trying to decouple
	// public SpecialProperties EnemyProperties{ 
	// 	get {
	// 		if(hitbox.EnemyObject != null){
	// 			Debug.Log("GotEnemy");
	// 			return hitbox.EnemyObject.GetComponentInParent<SpecialProperties>();
	// 		}
				
	// 		else
	// 			return null;
	// 	}
	// }

	//Probably the way to go
	public GameObject Target{
		get {
			if(target != null)
				if(target.GetComponent<SpecialProperties>())
					return target;
				else
					return null;
			else
				return null;		
		}
		set {
			if(value.GetComponent<SpecialProperties>())
				target = value;
			else
				target = null;	
			}
	}

	//Brainstorming
	public SpecialProperties TargetProperties{ 
		get {return target.GetComponent<SpecialProperties>();}
	}

	void Start(){
		body = GetComponent<Rigidbody2D> ();
		targetBody = GetComponent<Rigidbody2D>();
		//hitbox = GetComponentInChildren<HitboxController>();
		gravity = body.gravityScale;
		
	}

	void Update(){
	}

	//Common Calculations
	public float LaunchUnitHeight(float unitHeight){
		return Mathf.Sqrt(2 * unitHeight * Mathf.Abs(Physics2D.gravity.y) * targetBody.gravityScale);
	}

	//Special Properties
	public IEnumerator SetHitStun(float duration, bool freezeVelocity = true){
		hitStun = true;
		if(freezeVelocity) 
			StartCoroutine(SetFreezeVelocity());
		yield return new WaitForSeconds(duration);
		hitStun = false;
	}

	public IEnumerator SetFreezeVelocity(){
		Debug.Log("FROZEN");
		while(hitStun){
			targetBody.velocity = new Vector2(0,0);
			targetBody.gravityScale = 0;
			yield return new WaitForEndOfFrame();
		}
		targetBody.gravityScale = gravity;
	}

	void Launch(){
		targetBody.velocity = new Vector2(0, LaunchUnitHeight(launchHeight));		
	}

	public IEnumerator SetKnockBack(float xVelocity, float yVelocity, float duration){
		


		yield return new WaitForSeconds(duration);
	}
}
