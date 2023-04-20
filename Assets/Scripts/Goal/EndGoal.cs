using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGoal : MonoBehaviour
{
	public ScoringSystem _scoringSystem;

	private void OnTriggerEnter(Collider other)
    {
        if (Scores.totalScore == Scores.totalCoins)
        {
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene("GameWon");
		}
    }
}
