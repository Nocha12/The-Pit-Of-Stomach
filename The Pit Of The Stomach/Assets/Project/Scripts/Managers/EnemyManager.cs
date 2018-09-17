using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public PlayerHealth playerHealth;
	public PlayerHealth player2Health;
	public GameObject enemy;                
	public float spawnTime = 3f;            
	public Transform[] spawnPoints;         


	void Start ()
	{
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}


	void Spawn ()
	{
		spawnTime -= 0.02f;
		if (spawnTime < 0.5f)
			spawnTime = 0.5f;
		
		if(playerHealth.currentHealth <= 0 && (ScoreManager.isPlaySingle || player2Health.currentHealth <= 0))
		{
			return;
		}

		int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		GameObject e = Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		e.GetComponent<EnemyHealth> ().startingHealth += (int)(ScoreManager.score / 100) * 10;
		e.GetComponent<EnemyHealth> ().currentHealth += (int)(ScoreManager.score / 100) * 10;
	}
}