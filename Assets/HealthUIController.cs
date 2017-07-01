using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour {

	public Gradient gradient;

	HealthController healthController;
	Slider slider;

	float prevHealth;


	void Start(){
		slider = GetComponent<Slider>();
		healthController = GetComponent<HealthController>();
		UpdateBar(healthController, healthController.CurrentHealth, prevHealth, healthController.maxHealth);
	}

	void Update(){
		prevHealth = healthController.CurrentHealth;
		if(healthController.CurrentHealth < prevHealth){
			UpdateBar(healthController, healthController.CurrentHealth, prevHealth, healthController.maxHealth);
		}
	}

	public void UpdateBar(HealthController healthController, float health, float prevHealth, float maxHealth){
		if(healthController.gameObject.GetComponent<PlayerController>()){
			float percent = health / maxHealth;
			slider.value = percent;
		}
	}

}
