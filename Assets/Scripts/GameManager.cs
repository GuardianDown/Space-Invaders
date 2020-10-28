using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Text _scoreNumber = null;
	[SerializeField] private Image[] _liveIcons = null;
	[SerializeField] private AudioSource _invaderKilledSound = null;
	[SerializeField] private GameObject _resumeButton = null;
	[SerializeField] private GameObject _exitButton = null;
	
	private int _lives = 3;
	private int _score = 0;
	private Button _button;
	
	public static bool PlayerDiedPause { get; set; }
	
	private void Update()
	{
		PauseGame();
	}
	
	public void SubtractLives()
	{
		if(_lives == 0)
			ShowGameOverScene();
		else
		{
			_lives--;
			Destroy(_liveIcons[_lives]);
		}
	}
	
	public void ShowGameOverScene()
	{
		SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
	}
	
	public void ShowYouWinScene()
	{
		SceneManager.LoadScene("YouWin", LoadSceneMode.Single);
	}
	
	public void UpdateScore(int points)
	{
		_score += points;
		_scoreNumber.text = _score.ToString();
		_invaderKilledSound.Play();
	}
	
	private void PauseGame()
	{
		if(Input.GetButtonDown("Cancel"))
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				_resumeButton.SetActive(true);
				_exitButton.SetActive(true);;
			}
			else
			{
				ResumeGame();
			}
	}
	
	public void ResumeGame()
	{
		Time.timeScale = 1;
		_resumeButton.SetActive(false);
		_exitButton.SetActive(false);
	}
}
