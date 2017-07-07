using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public float damage = 1.0f;




	///Layers
	// 10 = ProjectileEnemy
	// 11 = ProjectilePlayer
	// 12 = Terrain
	// 	

	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.GetComponent<PlayerController>() && gameObject.layer == 11){
			AudioManager.PlayEffect("Hurt1");
			c.gameObject.GetComponent<HealthController>().CurrentHealth -= damage;
			gameObject.SetActive(false);		
		}
		if(c.gameObject.GetComponent<EnemyController>() && gameObject.layer == 10){
			AudioManager.PlayEffect("Hurt2");
			c.gameObject.GetComponent<HealthController>().CurrentHealth -= damage;
			gameObject.SetActive(false);
		}
		if(c.gameObject.layer == 12){
			gameObject.SetActive(false);
		}
		if(gameObject.layer == 11 && c.gameObject.layer == 10){
			AudioManager.PlayEffect("Hurt2");
			c.gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
	}
}
