using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform target;            
	public Transform target2;
	public float smoothing = 5f;        

	public Vector3 offset;                     

	void Start ()
	{
		InvokeRepeating ("SetOffset", 0, 5);
	}

	public void SetOffset()
	{
		Vector3 temp = new Vector3(0, 0, 0);
	
		temp += target.position;

		if(!ScoreManager.isPlaySingle)
			temp += target2.position;

		//if (target2.GetComponent<PlayerHealth> ().currentHealth > 0 && target.GetComponent<PlayerHealth> ().currentHealth > 0)
		if(!ScoreManager.isPlaySingle)
			temp /= 2;

		offset = transform.position - temp;
	}

	void FixedUpdate ()
	{
		Vector3 targetCamPos = new Vector3(0, 0, 0);

		//if (target.GetComponent<PlayerHealth> ().currentHealth > 0)
			targetCamPos += target.position;
		//if(target2.GetComponent<PlayerHealth> ().currentHealth > 0)
		if(!ScoreManager.isPlaySingle)
			targetCamPos += target2.position;

		//if (target2.GetComponent<PlayerHealth> ().currentHealth > 0 && target.GetComponent<PlayerHealth> ().currentHealth > 0)
		if(!ScoreManager.isPlaySingle)	
			targetCamPos /= 2;

		targetCamPos += offset;

		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}