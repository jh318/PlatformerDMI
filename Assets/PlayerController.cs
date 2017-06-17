using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;

	Rigidbody2D body;
	Animator anim;

	void Start(){
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Update(){
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		if (horizontal > 0) {
			transform.right = Vector3.right;
		} else if (horizontal < 0) {
			transform.right = -Vector3.right;
		}

		body.velocity = new Vector2(horizontal, body.velocity.y) * speed;

		anim.SetBool ("isRunning", Mathf.Abs (horizontal) > 0.1f);
	}

}
