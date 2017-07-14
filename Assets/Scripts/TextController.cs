﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

	public static TextController instance;

	public Text textUI;
	public GameObject textBox;
	public GameObject pressA;

	Queue<IEnumerator> queue = new Queue<IEnumerator>();


	void Awake(){
		if(instance == null){
			instance = this;
			pressA.SetActive(false);
			textUI.text = "";
		}
		else{
			Destroy(gameObject);
		}
	}

	void OnEnable(){
		StartCoroutine("ProcessQueue");
	}

	void Start(){
	TextController.ShowText("Show teeeext");
	TextController.TypeText("Type teeeext");
	TextController.WaitForInput();
	TextController.ClearText();
	TextController.TypeText("You waited");
	}

	IEnumerator ProcessQueue(){
		while(enabled){
			if(queue.Count > 0){
				textBox.SetActive(true);
				while(queue.Count > 0){
					yield return StartCoroutine(queue.Dequeue());
				}
				textBox.SetActive(false);
			}
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator ShowHide(bool show){
		textBox.SetActive(show);
		yield return new WaitForSeconds(0.5f);
	}

		public static void ClearText(){
			instance.queue.Enqueue(instance.ClearTextCoroutine());
	}

	IEnumerator ClearTextCoroutine(){
		textUI.text = "";
		yield return null;
	}

	public static void TypeText(string str){
		instance.queue.Enqueue(instance.TypeTextCoroutine(str));
	}

	IEnumerator TypeTextCoroutine (string str){
		for(int i = 0; i < str.Length; i++){
			textUI.text += str[i];
			yield return new WaitForSeconds(0.2f);
		}
	}

	public static void ShowText(string str){
		instance.queue.Enqueue(instance.ShowTextCoroutine(str));
	}

	IEnumerator ShowTextCoroutine(string str){
		textUI.text += str;
		yield return new WaitForSeconds(0.2f);
	}

	public static void WaitForInput(){
		instance.queue.Enqueue(instance.WaitForInputCoroutine());
	}

	IEnumerator WaitForInputCoroutine(){
		aPressed = false;
		pressA.SetActive(true);
		while(!aPressed){
			yield return new WaitForEndOfFrame();
		}
			pressA.SetActive(false);
	}

	bool aPressed = false;
	void Update(){
		if(Input.GetKeyDown(KeyCode.A)) aPressed = true;
	}
}
