using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour {

	public GameObject player;
	public Vector3 offset = new Vector3(0,0,-10);
	public float xLookAhead = 5.0f;

	float extensionRate;
	float compressionRate;
	float maxOffset;
	Vector3 currentFacing;
	Vector3 prevFacing;

	Vector3 lookAhead;

	Rigidbody2D body;

	float frac;

	void Start(){
		float starTime = Time.time;
		offset = new Vector3(0, 0, -10);
		offset = new Vector3(xLookAhead, 0, -10);
		//StartCoroutine(LerpLookAhead());
		StartCoroutine(SuperCamera());
		body = player.GetComponent<Rigidbody2D>();
	}

	void Update(){
		
		//LookAhead();
		//Camera.main.gameObject.transform.position = player.transform.position + offset;;

	}

	void LookAhead(){
		prevFacing = currentFacing;
		currentFacing = player.gameObject.transform.right;
		
		if(prevFacing != currentFacing){
			if(player.gameObject.transform.right == Vector3.right){
				
				xLookAhead = 5.0f;

			}
									
			if(player.gameObject.transform.right == -Vector3.right){
				xLookAhead = -5.0f;

			}
		}
	}

	void LookAheadTwo(){
		if(body.velocity.x > 0.1f){
			xLookAhead += lookAhead.x;
			Mathf.Clamp(xLookAhead, -5.0f, 5.0f);
		}
		else{
			xLookAhead -= lookAhead.x;
			Mathf.Clamp(xLookAhead, 0, 0);
		}
	}

	IEnumerator LerpLookAhead(){
		//float frac;
		while(true){
			for(float t = 0; t < 3.0f; t += Time.deltaTime){
				frac = t/3.0f;
				//lookAhead = Vector3.Lerp(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.position += new Vector3(xLookAhead,0,0), frac);
				//Debug.Log(Camera.main.gameObject.transform.position);
				//Debug.Log(Vector3.Lerp(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.position += lookAhead, frac));
				yield return new WaitForEndOfFrame();

			}
		}
	}


	IEnumerator SuperCamera(){
		while(true){
			float fraction;
			float t = 0.0f;
			Mathf.Clamp(t, 0, 3.0f);
			while(player.gameObject.transform.right == Vector3.right){
				t += Time.deltaTime;
				
				fraction = t /3.0f;
				Camera.main.transform.position = Vector3.Lerp(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.position += new Vector3(5.0f, 0, -10), frac);
				yield return new WaitForEndOfFrame();
			}
			while(player.gameObject.transform.right == -Vector3.right){
				t += Time.deltaTime;
				fraction = t /3.0f;
				Camera.main.transform.position = Vector3.Lerp(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.position += new Vector3(-5.0f, 0, -10), frac);
				yield return new WaitForEndOfFrame();

			}
			yield return new WaitForEndOfFrame();	
		}
	}

	

}