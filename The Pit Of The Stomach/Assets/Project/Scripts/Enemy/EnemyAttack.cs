using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
	public int attackDamage = 10;               // The amount of health taken away per attack.
	GameObject player;
	EnemyHealth enemyHealth;                    // Reference to this enemy's health.
	bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
	float timer;                                // Timer for counting up to the next attack.


	void Awake ()
	{
		enemyHealth = GetComponent<EnemyHealth>();
	}


	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player2"))
		{
			player = other.gameObject;
			playerInRange = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player)
		{
			player = null;
			playerInRange = false;
		}
	}


	void Update ()
	{
		timer += Time.deltaTime;

		if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
		{
			Attack ();
		}
	}


	void Attack ()
	{
		timer = 0f;
		PlayerHealth playerHealth = player.GetComponent<PlayerHealth> ();

		if(playerHealth.currentHealth > 0)
		{
			playerHealth.TakeDamage (attackDamage);
		}
	}
}