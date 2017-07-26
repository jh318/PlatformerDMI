using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.GetComponent<PlayerController>()){
			//TextController.gameObject.SetActive(true);
			TextController.TypeText("B to interact!");
			TextController.WaitForInput();
			TextController.ClearText();
			TextController.TypeText("X to Launch!");
			TextController.WaitForInput();
			TextController.ClearText();
			TextController.TypeText("Down to Interact with stuff! (Like Doors)");
			TextController.WaitForInput();
			TextController.ClearText();
			gameObject.SetActive(false);
		}
	}
}
