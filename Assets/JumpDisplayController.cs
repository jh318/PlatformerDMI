using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDisplayController : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
		transform.rotation = Quaternion.identity;
		if(PlayerController.instance.jumpCountMax - PlayerController.instance.JumpCount > 0 && PlayerController.instance.jumpCountMax - PlayerController.instance.JumpCount < 10)
			GetComponent<TextMesh>().text = "0" + (PlayerController.instance.jumpCountMax - PlayerController.instance.JumpCount).ToString();
		else if(PlayerController.instance.jumpCountMax - PlayerController.instance.JumpCount > 10){
			GetComponent<TextMesh>().text = (PlayerController.instance.jumpCountMax - PlayerController.instance.JumpCount).ToString();
		}
		else
			GetComponent<TextMesh>().text = "00";
	}
}
