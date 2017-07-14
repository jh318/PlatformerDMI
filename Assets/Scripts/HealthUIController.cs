using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour {



	Image healthBar;

	float prevHealth;


	void Awake(){
		healthBar = GetComponent<Image>();

	}

	void OnEnable () {	
		HealthController.onHealthChanged += UpdateBar;
	}

	void OnDisable () {
		HealthController.onHealthChanged -= UpdateBar;
	}

	public void UpdateBar(HealthController hc, float health, float prevHealth, float maxHealth){
		if(hc.gameObject.GetComponentInParent<PlayerController>()){
			healthBar.fillAmount = health / maxHealth;
		}
	}

}
