using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour {

	Vector3 startPos;

	float x = 0;
	Light boxLight;

	void Start () {
		startPos = transform.position;
		boxLight = GetComponent<Light> ();
	}

	void Update () {
		x += Time.deltaTime;
		transform.position = startPos + new Vector3 (0, Mathf.Sin(x * 3) / 4, 0);
		transform.Rotate (new Vector3 (0, 1, 0));
		boxLight.intensity = 1.5f + Mathf.Sin(x * 3) / 2;
	}
}
