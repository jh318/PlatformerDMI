using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour {



	Image healthBar;
	public Image healthBarDamageLayer;

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
			StopCoroutine("UpdateDamageBar");
			StartCoroutine("UpdateDamageBar");
		}
	}

	IEnumerator UpdateDamageBar(){
		for(float t = 0; t < 5.0; t += Time.deltaTime){
			float frac = t / 5.0f;
			healthBarDamageLayer.fillAmount = Mathf.Lerp(healthBarDamageLayer.fillAmount, healthBar.fillAmount, t);
			yield return new WaitForEndOfFrame();
		}
	}
}
