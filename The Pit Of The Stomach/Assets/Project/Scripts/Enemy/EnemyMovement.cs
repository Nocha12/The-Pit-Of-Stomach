using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	Transform player;               
	Transform player2;
	PlayerHealth playerHealth;
	PlayerHealth player2Health;
	EnemyHealth enemyHealth;    
	UnityEngine.AI.NavMeshAgent nav;


	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		if(!ScoreManager.isPlaySingle)
			player2 = GameObject.FindGameObjectWithTag ("Player2").transform;
		playerHealth = player.GetComponent <PlayerHealth> ();
		if(!ScoreManager.isPlaySingle)
			player2Health = player2.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
	}


	void Update ()
	{
		if(enemyHealth.currentHealth > 0)
		{
			if (!ScoreManager.isPlaySingle) {
				if (player2Health.currentHealth <= 0 || playerHealth.currentHealth > 0 && Vector3.Distance (player.position, transform.position) < Vector3.Distance (player2.position, transform.position))
					nav.SetDestination (player.position);
				else if (player2Health.currentHealth > 0)
					nav.SetDestination (player2.position);
			}
			else
				nav.SetDestination (player.position);
		}
		else
		{
			nav.enabled = false;
		}
	}
}