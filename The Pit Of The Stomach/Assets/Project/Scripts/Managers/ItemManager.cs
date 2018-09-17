using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
	public GameObject itemBox;
	public float spawnTime = 10f;

	float timer;

	public GameObject[] itemBoxes = new GameObject[8];
	Transform[] postions;

	void Awake()
	{
		postions = GetComponentsInChildren<Transform> ();
	}

	void Update () {
		timer += Time.deltaTime;

		if (timer >= spawnTime) {
			timer = 0;
			for (int i = 0; i < 8; i++) {
				int rand = Random.Range (0, 8);

				if (itemBoxes [rand] == null) {
					itemBoxes [rand] = Instantiate (itemBox, postions [rand + 1].position, Quaternion.identity);
					break;
				}
			}
		}
	}
}
