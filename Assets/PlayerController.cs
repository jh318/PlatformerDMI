using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float jumpForce = 200.0f;

	Rigidbody2D body;
	Animator anim;
	float horizontal;
	float vertical;
	List<GameObject> ground = new List<GameObject>();

	public bool isGrounded {
		get { return ground.Count > 0; }
	}

	void Start(){
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Update(){
		GetAxis ();
		FlipSprite ();
		SetVelocity ();
		SetAnimations ();
		CollisionCheck ();




	}


	void GetAxis(){
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
	}

	void FlipSprite(){
		if (horizontal > 0) {
			transform.right = Vector3.right;
		} 
		else if (horizontal < 0) {
			transform.right = -Vector3.right;
		}
	}

	void SetVelocity(){
		body.velocity = new Vector2(horizontal * speed, body.velocity.y);

		if (vertical > 0.1f && isGrounded) {
			body.AddForce (transform.up * jumpForce, ForceMode2D.Impulse);
		}
	}
		
	void SetAnimations(){
		anim.SetBool ("isRunning", Mathf.Abs (horizontal) > 0.1f);
		anim.SetBool ("isJumping", (!isGrounded));
		anim.SetBool ("isIdle", (isGrounded && body.velocity == new Vector2 (0, 0)));
		//if (isGrounded && !anim.GetBool("isJumping")) {
		//	anim.SetBool ("isLanding", true);
		//}
		//anim.SetBool("is", )
	}

	void CollisionCheck(){
		
	}


	///COLLISION
	//Check for Ground
	void OnCollisionEnter2D(Collision2D c){
		foreach (ContactPoint2D cp in c.contacts) {
			if (Vector2.Angle (cp.normal, Vector2.up) < 45) {
				ground.Add(c.gameObject);
				return;
			}
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		if (ground.Contains(c.gameObject)) {
			ground.Remove(c.gameObject);
		}
	}
}

