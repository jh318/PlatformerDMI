using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float jumpForce = 200.0f;
	public int jumpCountMax = 2;
	public BoxCollider2D hitbox;
	public float launchForce = 10.0f;


	Rigidbody2D body;
	Animator anim;

	List<GameObject> ground = new List<GameObject>();
	enum State{ground, air};
	State playerState;
	int jumpCount = 0;
	float horizontalInput;
	float verticalInput;
	float previousVerticalInput;
	float previousHorizontalInput;
	float gravity;
	HitboxController hitboxController;

	//ActionChecks
	bool attacking = false;
	bool slashChain1 = false;
	bool slashChain2 = false;


	public bool isGrounded {
		get { return ground.Count > 0; }
	}
	public bool canJump {
		get { return previousVerticalInput < 0.5f && verticalInput >= 0.5f; }
	}

	void Start(){
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		gravity = body.gravityScale;
		hitbox.gameObject.SetActive(false);
		hitboxController = hitbox.GetComponent<HitboxController>();
	}

	void Update(){
		GetAxis ();
		FlipSprite ();
		SetVelocity ();
		SetAnimations ();
		InputCheck();




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
				playerState = State.ground;
				return;
			}
		}
	}

	void PopGroundList(Collision2D c){
		if (ground.Contains(c.gameObject)) {
			ground.Remove(c.gameObject);
		}
	}

	void InputCheck(){
		if(verticalInput > 0.5f ){
			playerState = State.air;
		}

		if(Input.GetButtonDown("SlashButton") && !attacking){
			attacking = true;
			StartCoroutine("Slash1");
		}
		if(Input.GetButton("SlashButton") && slashChain1){
			StopCoroutine("Slash1");
			StartCoroutine("Slash2");
		}
		if(Input.GetButtonDown("SlashButton") && slashChain2){
			StopCoroutine("Slash2");
			StartCoroutine("Slash3");
		}
		if(Input.GetButtonDown("LauncherButton") && !attacking){
			StartCoroutine("Launcher");
		}

		if(attacking){
			StartCoroutine(FreezePosition());
		}
		
	}

	//Actions and Attacking
	IEnumerator Slash1(){
		anim.Play("Slash1");
		yield return new WaitForSeconds(0.15f);
		hitbox.gameObject.SetActive(true);
		HitStun(1.0f);
		yield return new WaitForSeconds(0.15f);
		hitbox.gameObject.SetActive(false);		
		//Check for Chain/Cancel
		slashChain1 = true;
		yield return new WaitForSeconds(0.4f); //Recovery 1
		//Extra Time to Chain/Cancel
		yield return new WaitForSeconds(0.4f); //Recovery 2 Final
		slashChain1 = false;
		attacking = false;
		anim.Play("Neutral");
	}

	IEnumerator Slash2(){
		slashChain1 = false;
		anim.Play("Slash2");
		yield return new WaitForSeconds(0.2f);
		hitbox.gameObject.SetActive(true);
		HitStun(1.0f);
		yield return new WaitForSeconds(0.2f);
		hitbox.gameObject.SetActive(false);		
		//Check for Chain/Cancel
		slashChain2 = true;
		yield return new WaitForSeconds(0.4f);

		yield return new WaitForSeconds(0.4f); //Recovery 2 Final
		slashChain2 = false;
		attacking = false;
		anim.Play("Neutral");
	}

	IEnumerator Slash3(){
		slashChain2 = false;
		anim.Play("Slash3");
		yield return new WaitForSeconds(0.1f);
		hitbox.gameObject.SetActive(true);
		HitStun(1.0f);
		yield return new WaitForSeconds(0.1f);
		hitbox.gameObject.SetActive(false);		
		//Check for Chain/Cancel
		slashChain1 = true;
		yield return new WaitForSeconds(0.4f); //Recovery 1
		//Extra Time to Chain/Cancel
		yield return new WaitForSeconds(0.4f); //Recovery 2 Final
		slashChain1 = false;
		attacking = false;
		anim.Play("Neutral");
	}

	IEnumerator Launcher(){
		anim.Play("Launcher");
		yield return new WaitForSeconds(0.1f);
		hitbox.gameObject.SetActive(true);
		Launch();
		yield return new WaitForSeconds(0.1f);
		hitbox.gameObject.SetActive(false);		
		//Check for Chain/Cancel
		slashChain1 = true;
		yield return new WaitForSeconds(0.1f); //Recovery 1
		//Extra Time to Chain/Cancel
		yield return new WaitForSeconds(0.1f); //Recovery 2 Final
		attacking = false;
		anim.Play("Neutral");
	}

	IEnumerator FreezePosition(){
		while(attacking){
			body.velocity = new Vector2(0,0);
			body.gravityScale = 0;
			yield return new WaitForEndOfFrame();
		}
		body.gravityScale = gravity;
	}

	//Special Properties
	void Launch(){
		if(hitboxController.EnemyHit){
			Debug.Log("TOBE");
			GameObject enemy = hitboxController.EnemyObject;
			Rigidbody2D enemyBody = enemy.GetComponent<Rigidbody2D>();
			//enemyBody.AddForce(transform.up * launchForce);
			enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, launchForce);
			//enemy.GetComponent<Rigidbody2D>().AddForce(transform.up * launchForce);

			hitboxController.EnemyHit = false;
		}
	}

	void HitStun(float duration){
		if(hitboxController.EnemyHit){
			GameObject enemy = hitboxController.EnemyObject;
			EnemyController enemyController = enemy.GetComponent<EnemyController>();
			
			StartCoroutine(enemyController.SetHitStun(duration));

		}
	}
}



