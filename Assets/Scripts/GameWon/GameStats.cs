using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStats : MonoBehaviour
{
	void Update()
	{
		GetComponent<TextMeshProUGUI>().text = "Collectibles Found: " + Scores.totalScore.ToString()
			+ " / " + Scores.totalCoins;
	}
}
