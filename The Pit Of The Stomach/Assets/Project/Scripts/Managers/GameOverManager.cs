using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
	public PlayerHealth playerHealth;    
	public PlayerHealth player2Health;

	Animator anim;                         
	public Image gameOverFader;

	void Awake ()
	{
		anim = GetComponent <Animator> ();
	}


	void Update ()
	{
		if(playerHealth.currentHealth <= 0 && (ScoreManager.isPlaySingle || player2Health.currentHealth <= 0))
		{
			anim.SetTrigger ("GameOver");
		
			if(gameOverFader.color.a >= 0.5f && Input.GetKeyDown(KeyCode.Return))
				SceneManager.LoadScene (0);		
		}
	}
}