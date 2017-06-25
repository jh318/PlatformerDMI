using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour {

	public GameObject target;
	public Vector3 offset = new Vector3(0,0,-10);
	public float xLookAhead = 5.0f;
	public float facingExtensionRateX = 8.0f;
	public float facingExtensionRateY = 12.0f;
	public float maxOffsetX = 5.0f;
	public float maxOffsetY = 3.0f;

	//float compressionRate = 6.0f;

	Vector3 lookAhead;
	float movingOffsetX = 0;
	float movingOffsetY = 0;

	void LateUpdate(){
		OffsetCameraToMovementX ();
		OffsetCameraToMovementY ();

		movingOffsetX = Mathf.Clamp (movingOffsetX, -maxOffsetX, maxOffsetX);
		movingOffsetY = Mathf.Clamp (movingOffsetY, -maxOffsetY, maxOffsetY);

		Vector3 camPos = target.transform.position + new Vector3 (movingOffsetX, movingOffsetY, 0);
		camPos.z = transform.position.z;

		transform.position = camPos;
	}

	void OffsetCameraToMovementX(){
		if (target.GetComponent<Rigidbody2D> ().velocity.x > 0.1f) {
			movingOffsetX += facingExtensionRateX * Time.deltaTime;
		} 
		else if (target.GetComponent<Rigidbody2D> ().velocity.x < -0.1f) {
			movingOffsetX -= facingExtensionRateX * Time.deltaTime;
		} 
		else {
			movingOffsetX += 0;
		}
	}

	void OffsetCameraToMovementY(){
		if (target.GetComponent<Rigidbody2D> ().velocity.y > 0.1f) {
			movingOffsetY += facingExtensionRateY * Time.deltaTime;
		} 
		else if (target.GetComponent<Rigidbody2D>().velocity.y < -0.1f) {
			movingOffsetY -= facingExtensionRateY * Time.deltaTime;
		} 
		else {
			movingOffsetY = Mathf.Lerp(movingOffsetY, 0, Time.deltaTime);
		}
	}
}