using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {
	public Image howToPlay;
	public Image story;

	void Update()
	{
		if (howToPlay.enabled && Input.GetMouseButtonDown(0))
			howToPlay.gameObject.SetActive (false);
		if (story.enabled && Input.GetMouseButtonDown(0))
			story.gameObject.SetActive (false); 
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ViewHowToPlay()
	{
		howToPlay.gameObject.SetActive (true);
	}

	public void ViewStory()
	{
		story.gameObject.SetActive (true);
	}

	public void ChangePlayerOneScene()
	{
		ScoreManager.score = 0;
		ScoreManager.isPlaySingle = true;
		SceneManager.LoadScene (1);
	}
	public void ChangePlayerTwoScene()
	{
		ScoreManager.score = 0;
		ScoreManager.isPlaySingle = false;
		SceneManager.LoadScene (2);
	}
}
