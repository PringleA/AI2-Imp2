using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Scores
{
	public static int totalScore = 0;
	public static int totalCoins = 0;
    public static int totalEnemiesKilled = 0;
    public static int totalEnemies = 0;

	public static void AddScore(int amount)
	{
		totalScore += amount;
	}

    public static void AddKill(int amount)
    {
        totalEnemiesKilled += amount;
    }

	public static void ResetScores()
	{
		totalScore = 0;
		totalEnemies = 0;
		totalEnemiesKilled = 0;
		totalEnemies = 0;
	}

	public static void InitScores()
	{
		totalCoins = GameObject.FindGameObjectsWithTag("Collectible").Length;
		totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
	}
}

public class ScoringSystem : MonoBehaviour
{
    public GameObject scoreText;
	public GameObject enemyText;
	//public AudioSource collectSound;

	private void Awake()
    {
		Scores.ResetScores();
		Scores.InitScores();
	}

    void Update()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Collectibles Found: " + Scores.totalScore.ToString()
            + " / " + Scores.totalCoins;
		enemyText.GetComponent<TextMeshProUGUI>().text = "Enemies Killed: " + Scores.totalEnemiesKilled.ToString()
			+ " / " + Scores.totalEnemies;
	}
}
