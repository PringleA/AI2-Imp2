using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {

	}

    public void PlayGame()
    {
		Cursor.lockState = CursorLockMode.Locked;
        Scores.ResetScores();
        Scores.InitScores();
		SceneManager.LoadScene("PlayGame");
	}

    public void PlayMenu()
    {
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
