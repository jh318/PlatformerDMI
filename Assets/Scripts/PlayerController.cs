using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;


	public float speed = 5.0f;
	public float jumpForce = 200.0f;
	public int jumpCountMax = 2;
	public BoxCollider2D hitbox1;
	public float launchForce = 10.0f;
	public float jumpHeight = 2;
	public float launchHeight = 2;


	Rigidbody2D body;
	Animator anim;
	SpecialProperties sProperties;

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
	HealthController healthController;
	//Vector2 v;

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

	void Awake(){
		if(instance == null){
			instance = this;
		}
	}

	void Start(){
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		sProperties = GetComponent<SpecialProperties>();
		gravity = body.gravityScale;
		hitbox1.gameObject.SetActive(false);
		hitboxController = hitbox1.GetComponent<HitboxController>();
		healthController = GetComponent<HealthController>();
	}

	void Update(){
		GetAxis ();
		InputCheck();
		FlipSprite ();
		SetVelocity ();
		SetAnimations ();
		Death();
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
			body.velocity = new Vector2(body.velocity.x, JumpVelocity(jumpHeight));
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

	public float JumpVelocity(float height){
		return Mathf.Sqrt(2 * height * Mathf.Abs(Physics2D.gravity.y) * body.gravityScale);
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
			//sProperties.Target = this.gameObject;
			//StartCoroutine(sProperties.SetFreezeVelocity());
			StartCoroutine(FreezePosition());
		}
		
	}

	//Actions and Attacking
	IEnumerator Slash1(){
		anim.Play("Slash1");
		AudioManager.PlayEffect("Slash1");
		//"StartUp"
		hitbox1.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.02f);
		 //"Active"
		//if (sProperties.Targets.Count == 0) Debug.Log("MISS, Slash1");
		// foreach (GameObject target in sProperties.Targets) {
		// 	StartCoroutine(target.GetComponent<SpecialProperties>().SetHitStun(0.1f));
		// }
		foreach(GameObject target in sProperties.Targets){
			AudioManager.PlayEffect("Deflect");
			target.GetComponent<SpecialProperties>().ReflectProjectile();
			Damage(3, target);
		}
		hitbox1.gameObject.SetActive(false);
		yield return new WaitForEndOfFrame();

		yield return new WaitForSeconds(0.05f); //"Active"
		slashChain1 = true; 


		yield return new WaitForSeconds(0.5f); //"Recovery"
		slashChain1 = false;
		attacking = false;
		anim.Play("Neutral");
	}

	IEnumerator Slash2(){
		slashChain1 = false;
		anim.Play("Slash2");
		AudioManager.PlayEffect("Slash2");
		hitbox1.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.03f);
		foreach (GameObject target in sProperties.Targets) {
			StartCoroutine(target.GetComponent<SpecialProperties>().SetHitStun(0.09f));
			Damage(3, target);

		}
		hitbox1.gameObject.SetActive(false);
		yield return new WaitForEndOfFrame();		
		yield return new WaitForSeconds(0.1f);
		slashChain2 = true;		//Check for Chain/Cancel

		//Extra Cancel time
		yield return new WaitForSeconds(0.4f); //Recovery 2 Final
		slashChain2 = false;
		attacking = false;
		anim.Play("Neutral");
	}

	IEnumerator Slash3(){
		slashChain2 = false;
		anim.Play("Slash3");
		AudioManager.PlayEffect("Slash3");
		hitbox1.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.03f);
		// foreach (GameObject target in sProperties.Targets) {
		// 	StartCoroutine(target.GetComponent<SpecialProperties>().SetHitStun(1.0f));
		// }
		foreach(GameObject target in sProperties.Targets){
			StartCoroutine(target.GetComponent<SpecialProperties>().SetKnockBack((5.0f)*transform.right.x,5.0f));
			Damage(4, target);

		}
		hitbox1.gameObject.SetActive(false);
		yield return new WaitForEndOfFrame();	
		yield return new WaitForSeconds(0.4f); //Recovery 1
		//Extra Time to Chain/Cancel
		yield return new WaitForSeconds(0.4f); //Recovery 2 Final
		attacking = false;
		anim.Play("Neutral");
	}

	IEnumerator Launcher(){
		anim.Play("Launcher");
		AudioManager.PlayEffect("Launch");
		hitbox1.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame(); //Wait for OnEnable()
		yield return new WaitForSeconds(0.1f); //Active
		if (sProperties.Targets.Count == 0) Debug.Log("MISS, Launcher");
		if(sProperties.Targets.Count > 0){
			foreach (GameObject target in sProperties.Targets) {
			StartCoroutine(target.GetComponent<SpecialProperties>().Launcher());
			Damage(3, target);
			}
		}
		hitbox1.gameObject.SetActive(false); 
		yield return new WaitForEndOfFrame();//Disable hitbox and Begin Recovery
		yield return new WaitForSeconds(0.1f); //Recovery 1
		//Extra Time to Chain/Cancel
		yield return new WaitForSeconds(0.1f); //Recovery 2 Final
		attacking = false;
		anim.Play("Neutral");
	}

	//Other Stuff
	IEnumerator FreezePosition(){
		while(attacking){
			body.velocity = new Vector2(0,0);
			body.gravityScale = 0;
			yield return new WaitForEndOfFrame();
		}
		body.gravityScale = gravity;
	}

	void Death(){
		if(healthController.CurrentHealth <= 0){
			gameObject.SetActive(false);
		}
	}

	void Damage(float damage, GameObject target){
		if(target.GetComponentInParent<HealthController>()){
			target.GetComponentInParent<HealthController>().CurrentHealth -= damage;
			Debug.Log(target.GetComponentInParent<HealthController>().CurrentHealth);
		}
	}

	void OnDisable(){
		Debug.Log("PlayerOnDisable");
		SceneLoader.instance.LoadScene("MainMenu");
	}
}