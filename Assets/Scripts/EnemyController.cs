using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	Rigidbody2D body;
	public string projectileName;
	public bool shoot = false;
	public Vector2 startVelocity = new Vector2(1,0);
	public Vector2 projectileVelocity = new Vector2(0,0);
	HealthController healthController;

	float gravity;

	void Start(){
		body = GetComponent<Rigidbody2D>();
		gravity = body.gravityScale;
		body.velocity = startVelocity;
		healthController = GetComponent<HealthController>();
		
		if(shoot)
			StartCoroutine(ShootProjectileCoroutine(2.0f));
	}

	void Update(){
		//body.velocity = new Vector3(1,body.velocity.y,0);
		Death();
	}

	void ShootProjectile(string enemyProjectile){
		GameObject projectile = Spawner.Spawn(enemyProjectile);
		projectile.layer = 11;
		projectile.transform.position = transform.position;
		projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(projectileVelocity.x * transform.right.x, projectileVelocity.y);
	}

	IEnumerator ShootProjectileCoroutine(float duration){
		while(shoot){
			ShootProjectile("Projectile");	
			yield return new WaitForSeconds(duration);
		}	
	}
	
	void Death(){
		if(healthController.CurrentHealth <= 0){
			gameObject.SetActive(false);
			PlayerController.instance.JumpCount--;
		}
	}
}
