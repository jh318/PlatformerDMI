using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	public float maxHealth;

	float currentHealth;

	public float CurrentHealth{
		get {return currentHealth; }
		set {currentHealth = value; }
	}

	void Start(){
		currentHealth = maxHealth;
	}
}
