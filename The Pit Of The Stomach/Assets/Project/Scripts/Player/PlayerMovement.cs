using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public bool isPlayer1 = true;
	public float speed = 6f;            // The speed that the player will move at.

	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	PlayerShooting playerShooting;

	void Awake ()
	{
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		playerShooting = GetComponentInChildren<PlayerShooting> ();
	}


	void FixedUpdate ()
	{
		float h = 0, v = 0;
		if (isPlayer1) {
			if (Input.GetKey (KeyCode.W))
				v = 1;
			if (Input.GetKey (KeyCode.S))
				v = -1;
			if (Input.GetKey (KeyCode.A))
				h = -1;
			if (Input.GetKey (KeyCode.D))
				h = 1;
		} else {
			if (Input.GetKey (KeyCode.UpArrow))
				v = 1;
			if (Input.GetKey (KeyCode.DownArrow))
				v = -1;
			if (Input.GetKey (KeyCode.LeftArrow))
				h = -1;
			if (Input.GetKey (KeyCode.RightArrow))
				h = 1;
		}

		Move (h, v);
		Animating (h, v);
	}


	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		if(transform.position != transform.position + movement)
			Turning (movement);
		
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning (Vector3 movement)
	{
		Vector3 vec = (transform.position + movement) - transform.position;
		Quaternion newRotatation = Quaternion.LookRotation(vec);
		playerRigidbody.MoveRotation(newRotatation);
	}
		
	void Animating (float h, float v)
	{
		bool walking = h != 0f || v != 0f;

		anim.SetBool ("IsWalking", walking);
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("ItemBox")) {
			PlayerHealth playerHealth = GetComponent<PlayerHealth> ();

			playerHealth.playerAudio.clip = playerHealth.getItemClip;
			playerHealth.playerAudio.Play ();

			playerShooting.GetItemBox ();
			if (playerHealth.currentHealth > 0)
				playerHealth.currentHealth = 100;
			playerHealth.healthSlider.value = playerHealth.currentHealth;

			Destroy (other.gameObject);
		}
	}
}