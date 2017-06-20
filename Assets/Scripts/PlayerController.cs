using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float jumpForce = 200.0f;
	public int jumpCountMax = 2;


	Rigidbody2D body;
	Animator anim;

	List<GameObject> ground = new List<GameObject>();
	int jumpCount = 0;
	float horizontalInput;
	float verticalInput;
	float previousVerticalInput;
	float previousHorizontalInput;

	public bool isGrounded {
		get { return ground.Count > 0; }
	}
	public bool canJump {
		get { return previousVerticalInput < 0.5f && verticalInput >= 0.5f; }
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
		horizontalInput = Input.GetAxisRaw("Horizontal");
		previousVerticalInput = verticalInput;
		verticalInput = Input.GetAxisRaw ("Vertical");
	}

	void FlipSprite(){
		if (horizontalInput > 0) {
			transform.right = Vector3.right;
		} 
		else if (horizontalInput < 0) {
			transform.right = -Vector3.right;
		}
	}

	void SetVelocity(){
		body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

		if (canJump && (isGrounded || jumpCount < jumpCountMax)) {
			jumpCount++;
			body.velocity = new Vector2(body.velocity.x, jumpForce);
		}
	}
		
	void SetAnimations(){
		anim.SetBool ("isRunning", Mathf.Abs (horizontalInput) > 0.1f);
		anim.SetBool ("isJumping", body.velocity.y > 0.5f);
		anim.SetBool ("isIdle", (isGrounded && body.velocity.magnitude < 0.1f));
		anim.SetBool ("isGrounded", isGrounded);
	}
		

	///COLLISION
	void OnCollisionEnter2D(Collision2D c){
		CheckForGround(c);
	}

	void OnCollisionExit2D(Collision2D c) {
		PopGroundList(c);
	}

	//Collision Functions
	void CheckForGround(Collision2D c){
		foreach (ContactPoint2D cp in c.contacts) {
			if (Vector2.Angle (cp.normal, Vector2.up) < 45) {
				ground.Add(c.gameObject);
				jumpCount = 0;
				return;
			}
		}
	}

	void PopGroundList(Collision2D c){
		if (ground.Contains(c.gameObject)) {
			ground.Remove(c.gameObject);
		}
	}
}

