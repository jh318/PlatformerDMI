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
	GameObject target;
	List<GameObject> targets = new List<GameObject>();


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
	public IEnumerator SetHitStun(float duration, bool freezeVelocity = true){
		hitStun = true;
		if(freezeVelocity) 
			StartCoroutine(SetFreezeVelocity());
		yield return new WaitForSeconds(duration);
		hitStun = false;
	}

	public IEnumerator SetFreezeVelocity(){
		while(hitStun){
			body.velocity = new Vector2(0,0);
			body.gravityScale = 0;
			yield return new WaitForEndOfFrame();
		}
		body.gravityScale = gravity;
	}

	public void Launch(){
		body.velocity = new Vector2(0, LaunchUnitHeight(launchHeight));		
	}

	public IEnumerator SetKnockBack(float xVelocity, float yVelocity, float duration){
		


		yield return new WaitForSeconds(duration);
	}
}
