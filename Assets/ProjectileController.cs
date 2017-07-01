using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public float damage = 1.0f;

	

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.GetComponent<PlayerController>() && gameObject.layer == 11){
			c.gameObject.GetComponent<HealthController>().CurrentHealth -= damage;
			gameObject.SetActive(false);		
		}
		if(c.gameObject.GetComponent<EnemyController>() && gameObject.layer == 10){
			c.gameObject.GetComponent<HealthController>().CurrentHealth -= damage;
			gameObject.SetActive(false);
		}
	}
}
