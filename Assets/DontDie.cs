using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDie : MonoBehaviour {

	
	void OnDisable()
	{
		StartCoroutine("Revive");
	}

	IEnumerator Revive(){
		yield return new WaitForSeconds(3);
		gameObject.SetActive(true);
	}
}
