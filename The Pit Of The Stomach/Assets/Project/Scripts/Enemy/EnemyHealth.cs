using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	public int startingHealth = 100;            // The amount of health the enemy starts the game with.
	public int currentHealth;                   // The current health the enemy has.
	public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
	public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
	public AudioClip deathClip;                 // The sound to play when the enemy dies.

	Animator anim;                              // Reference to the animator.
	AudioSource enemyAudio;                     // Reference to the audio source.
	ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
	CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
	bool isDead;                                // Whether the enemy is dead.
	bool isSinking;                             // Whether the enemy has started sinking through the floor.
	Color[] originalColors = new Color[50];

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		enemyAudio = GetComponent <AudioSource> ();
		hitParticles = GetComponentInChildren <ParticleSystem> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();

		currentHealth = startingHealth;

		int index = 0;
		SkinnedMeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (SkinnedMeshRenderer renderer in meshRenderers)
		{
			Material[] materials = renderer.materials;
			for(int i = 0; i < materials.Length; i++) 
				originalColors[index++] = materials [i].color;
		}
	}


	void Update ()
	{
		if(isSinking)
		{
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}


	public void TakeDamage (int amount, Vector3 hitPoint)
	{
		if(isDead)
			return;

		enemyAudio.Play ();

		currentHealth -= amount;

		FlashOut (gameObject);

		hitParticles.transform.position = hitPoint;

		hitParticles.Play();

		if(currentHealth <= 0)
		{
			Death ();
		}
	}


	public void FlashOut(GameObject gameObject)
	{
		SkinnedMeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
		StartCoroutine (RendererFlashOut(meshRenderers));
	}

	IEnumerator RendererFlashOut (SkinnedMeshRenderer[] meshRenderers) {
		int index = 0;
		foreach (SkinnedMeshRenderer renderer in meshRenderers)
		{
			Material[] materials = renderer.materials;
			for(int i = 0; i < materials.Length; i++) {
				materials [i].color = new Color (255, 0, 0, 1);
			}
		}

		while(true) {
			index = 0;
			bool flag = false;
			foreach (SkinnedMeshRenderer renderer in meshRenderers) {
				if (!renderer)
					break; 
				Material[] materials = renderer.materials;
				for(int i = 0; i < materials.Length; i++) {
					materials [i].color = Color.Lerp(materials [i].color, originalColors[index], 10 * Time.deltaTime);
					if (Mathf.Abs(materials [i].color.b - originalColors [index].b) < 0.01f)
						flag = true;
					index++;
				}
			}
			yield return null;
			if (flag) {
				index = 0;
				foreach (SkinnedMeshRenderer renderer in meshRenderers)
				{	
					Material[] materials = renderer.materials;
					for(int i = 0; i < materials.Length; i++) {
						materials [i].color = originalColors[index++];
					}
				}
				break;
			}
		}
	}

	void Death ()
	{
		isDead = true;

		capsuleCollider.isTrigger = true;
		anim.SetTrigger ("Dead");

		enemyAudio.clip = deathClip;
		enemyAudio.Play ();
	}


	public void StartSinking ()
	{
		GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
		GetComponent <Rigidbody> ().isKinematic = true;

		isSinking = true;

		ScoreManager.score += scoreValue;

		Destroy (gameObject, 2f);
	}
}