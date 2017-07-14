using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	public float maxHealth;

	float currentHealth;

	public delegate void OnHealthChanged(HealthController hc, float health, float prevHealth, float maxHealth);
	public static event OnHealthChanged onHealthChanged = delegate{};

	public float CurrentHealth{
		get {return currentHealth; }
		set {
			if (currentHealth != value) {
				onHealthChanged(this, value, currentHealth, maxHealth);
				currentHealth = value;
			}
		}
	}

	void Start(){
		CurrentHealth = maxHealth;
	}
}
