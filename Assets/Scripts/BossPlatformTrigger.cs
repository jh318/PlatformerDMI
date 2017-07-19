using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatformTrigger : MonoBehaviour {

	public GameObject bossPlatform;

	void OnCollisionExit2D(){
		//gameObject.SetActive(false);
		//bossPlatform.SetActive(false);
	}

	void OnCollisionEnter2D(){
		StartCoroutine("BossIntroCutscene");
	}

	IEnumerator BossIntroCutscene(){
		yield return new WaitForEndOfFrame();
		UIManager.instance.textBox.SetActive(true);
		TextController.TypeText("Muhuahahaha~~!");
		TextController.WaitForInput();
		TextController.ClearText();
		TextController.TypeText("MUHUAHAHAHAAH~~!!");
		TextController.WaitForInput();
		TextController.ClearText();		
		while(TextController.queueSize > 0) {
			yield return new WaitForEndOfFrame();
		}
		bossPlatform.SetActive(false);
	}
}
