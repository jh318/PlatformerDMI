using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpecialProperties : MonoBehaviour {
	
	//Common Components
	Rigidbody2D body;
	HitboxController hitbox; //Need a box trigger on a child object with this script
	
	//Common Variables
	GameObject enemyObject;
	bool hitStun = false;
	float gravity;
	float launchHeight;

	public SpecialProperties EnemyObject{
		get {
				if(enemyObject != null){
					return enemyObject.GetComponentInParent<SpecialProperties>();
			}else{
				return null;
			}
		}
	}

	void Start(){
		body = GetComponent<Rigidbody2D> ();
		//if(GetComponentInChildren<HitboxController>())
		hitbox = GetComponentInChildren<HitboxController>();

		gravity = body.gravityScale;
	}

	void Update(){
		if(enemyObject != null)
			enemyObject = hitbox.EnemyObject;
	}

	//Common Calculations
	public float LaunchUnitHeight(float unitHeight){
		return Mathf.Sqrt(2 * unitHeight * Mathf.Abs(Physics2D.gravity.y) * body.gravityScale);
	}

	void clearEnemy(){
		enemyObject = null;
	}

	//Special Properties
	public IEnumerator SetHitStun(float duration, bool freezeVelocity = false){
		hitStun = true;
		if(freezeVelocity) StartCoroutine(SetFreezeVelocity());
		yield return new WaitForSeconds(duration);
		hitStun = false;
		clearEnemy();
	}

	public IEnumerator SetFreezeVelocity(){
		while(hitStun){
			body.velocity = new Vector2(0,0);
			body.gravityScale = 0;
			yield return new WaitForEndOfFrame();
		}
		body.gravityScale = gravity;
		clearEnemy();
	}

	void Launch(){
		if(hitbox.EnemyHit){
			GameObject enemy = hitbox.EnemyObject;
			enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, LaunchUnitHeight(launchHeight));
			hitbox.EnemyHit = false;
		}
		clearEnemy();
	}

	public IEnumerator SetKnockBack(float xVelocity, float yVelocity, float duration){
		


		yield return new WaitForSeconds(duration);
	}
}
