using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float jumpForce = 200.0f;
	public int jumpCountMax = 2;
	private int jumpCount = 0;

	Rigidbody2D body;
	Animator anim;
	List<GameObject> ground = new List<GameObject>();
	float horizontal;
	float vertical;
	float lastVertical;
	public bool isGrounded {
		get { return ground.Count > 0; }
	}

	public bool shouldJump {
		get { return lastVertical < 0.5f && vertical >= 0.5f; }
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




	}


	void GetAxis(){
		horizontal = Input.GetAxisRaw("Horizontal");
		lastVertical = vertical;
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

		if (shouldJump && (isGrounded || jumpCount < jumpCountMax)) {
			jumpCount++;
			body.velocity = new Vector2(body.velocity.x, jumpForce);
		}
	}
		
	void SetAnimations(){
		anim.SetBool ("isRunning", Mathf.Abs (horizontal) > 0.1f);
		anim.SetBool ("isJumping", body.velocity.y > 0.5f);
		anim.SetBool ("isIdle", (isGrounded && body.velocity.magnitude < 0.1f));
		anim.SetBool ("isGrounded", isGrounded);
	}

	void SetAttributes(){
		if (isGrounded)
			jumpCount = 2;
	}
		

	///COLLISION
	//Check for Ground
	void OnCollisionEnter2D(Collision2D c){
		foreach (ContactPoint2D cp in c.contacts) {
			if (Vector2.Angle (cp.normal, Vector2.up) < 45) {
				ground.Add(c.gameObject);
				jumpCount = 0;
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

