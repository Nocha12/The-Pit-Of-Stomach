using UnityEngine;
using UnityEngine.UI;

enum GunType {
	NORAML,
	UZI,
	SHOOTGUN,
	RAILGUN
}

public class PlayerShooting : MonoBehaviour
{
	public int damagePerShot = 20;                
	public float range = 100f;                    

	private int[] magazine = new int[] { 1, 0, 0, 0 } ;
	private float[] timeBetweenBullets = new float[] { 0.3f, 0.08f, 1, 2 } ;
	private float[] minTimeBetweenBullets = new float[] { 0.15f, 0.04f, 0.5f, 1 } ;

	float timer;                                  
	Ray shootRay = new Ray();                     
	RaycastHit shootHit;                          
	int shootableMask;                            
	ParticleSystem gunParticles;                  
	LineRenderer gunLine;                         
	AudioSource gunAudio;
	public AudioClip changeWeaponClip;
	public AudioClip shootGunClip;
	Light gunLight;                               
	public Light faceLight;						
	float effectsDisplayTime = 0.1f;            

	public LineRenderer[] gunLines = new LineRenderer[7]; 
	public Text gunText;

	GunType gunType = GunType.NORAML;
	string[] gunString = { "Gun : ", "UZI : ", "ShootGun : ", "RailGun : " };
	PlayerMovement playerMovement;

	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Shootable");

		playerMovement = transform.parent.GetComponent<PlayerMovement> ();
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}

	void ChangeWeapon()
	{
		gunAudio.clip = changeWeaponClip;
		gunAudio.Play ();

		for (int i = (int)gunType + 1; i <= (int)GunType.RAILGUN; i++) {
			if (magazine [i] > 0) {
				gunType = (GunType)i;

				return;
			}
		}
		gunType = GunType.NORAML;
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (((playerMovement.isPlayer1 && Input.GetKey (KeyCode.V)) || (!playerMovement.isPlayer1 && Input.GetKey (KeyCode.L))) && timer >= timeBetweenBullets [(int)gunType] && Time.timeScale != 0) {
			if (gunType == GunType.SHOOTGUN) {
				ShootShootGun ();
				timeBetweenBullets [(int)gunType] -= 0.01f;
			}
			if (gunType == GunType.RAILGUN) {
				ShootRailGun ();
				timeBetweenBullets [(int)gunType] -= 0.05f;
			} else {
				Shoot ();
				timeBetweenBullets [(int)gunType] -= 0.0005f;
			}

			if (minTimeBetweenBullets [(int)gunType] > timeBetweenBullets [(int)gunType])
				timeBetweenBullets [(int)gunType] = minTimeBetweenBullets [(int)gunType];
		}
		if(((playerMovement.isPlayer1 && Input.GetKeyDown(KeyCode.B)) || (!playerMovement.isPlayer1 && Input.GetKeyDown(KeyCode.Semicolon))))
		{
			ChangeWeapon ();
		}

		if(timer >= timeBetweenBullets[(int)gunType] * effectsDisplayTime)
		{
			DisableEffects ();
		}
		if(gunType != GunType.NORAML)
			gunText.text = gunString [(int)gunType] + magazine[(int)gunType].ToString();
		else
			gunText.text = gunString [(int)gunType] + "∞";
	}

	public void GetItemBox()
	{
		GunType type = (GunType)Random.Range ((int)GunType.UZI, (int)GunType.RAILGUN + 1);
		if (type == GunType.UZI)
			magazine [(int)type] += 100 + (int)(ScoreManager.score / 100) * 10;
		if(type == GunType.SHOOTGUN)
			magazine [(int)type] += 40 + (int)(ScoreManager.score / 100) * 5;
		if(type == GunType.RAILGUN)
			magazine [(int)type] += 5 + (int)(ScoreManager.score / 100);
	}

	public void DisableEffects ()
	{
		gunLine.enabled = false;
		for(int i = 0 ; i < 7; i++)
			gunLines[i].enabled = false;
		gunLine.enabled = false;
		faceLight.enabled = false;
		gunLight.enabled = false;
	}

	void ShootRailGun ()
	{
		timer = 0f;

		gunAudio.clip = shootGunClip;
		gunAudio.Play ();

		gunLight.enabled = true;
		faceLight.enabled = true;

		gunParticles.Stop ();
		gunParticles.Play ();

		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		RaycastHit[] hits;
		hits = Physics.RaycastAll (shootRay, range, shootableMask);

		for (int i = 0; i < hits.Length; i++)
		{
			EnemyHealth enemyHealth = hits[i].collider.GetComponent <EnemyHealth> ();

			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (10000, hits[i].point);
				enemyHealth.GetComponent<Rigidbody> ().velocity = shootRay.direction * 30;
			}
		}

		gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);

		if (gunType != GunType.NORAML) {
			if (--magazine [(int)gunType] <= 0)
				ChangeWeapon ();
		}
	}

	void ShootShootGun ()
	{
		timer = 0f;

		gunAudio.clip = shootGunClip;
		gunAudio.Play ();

		gunLight.enabled = true;
		faceLight.enabled = true;

		gunParticles.Stop ();
		gunParticles.Play ();

		for (int i = 0; i < gunLines.Length; i++) {
			gunLines[i].enabled = true;
			gunLines[i].SetPosition (0, transform.position);
		}

		for (int i = 0; i < gunLines.Length; i++) {
			shootRay.origin = transform.position;
			shootRay.direction = transform.forward + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));

			if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
				EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

				if (enemyHealth != null) {
					enemyHealth.TakeDamage (damagePerShot, shootHit.point);
					enemyHealth.GetComponent<Rigidbody> ().velocity = shootRay.direction * 30;
				}

				gunLines[i].SetPosition (1, shootHit.point);
			} else {
				gunLines[i].SetPosition (1, shootRay.origin + shootRay.direction * range);
			}
		}
		if (gunType != GunType.NORAML) {
			if (--magazine [(int)gunType] <= 0)
				ChangeWeapon ();
		}
	}

	void Shoot ()
	{
		timer = 0f;

		gunAudio.clip = shootGunClip;
		gunAudio.Play ();

		gunLight.enabled = true;
		faceLight.enabled = true;

		gunParticles.Stop ();
		gunParticles.Play ();

		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
				enemyHealth.GetComponent<Rigidbody> ().velocity = shootRay.direction * 100;
			}

			gunLine.SetPosition (1, shootHit.point);
		}
		else
		{
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}

		if (gunType != GunType.NORAML) {
			if (--magazine [(int)gunType] <= 0)
				ChangeWeapon ();
		}
	}
}
