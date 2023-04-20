using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlaceCoins : MonoBehaviour
{
    GameObject[] coins;
    GameObject grid;
	Vector3 randPos;

	// Start is called before the first frame update
	void Start()
    {
        coins = GameObject.FindGameObjectsWithTag("Collectible");
        grid = GameObject.FindGameObjectWithTag("Grid");

		// get grid size and scale to spread coins over
		float gridSize;
        float spreadScale = 9.0f;
        // y position of collectibles is always the same
        float yPos = 1.0f;
        // get extents of grid in level for range
		gridSize = grid.transform.localScale.x;

        float minRange = -(gridSize * spreadScale) / 2;
		float maxRange = (gridSize * spreadScale) / 2;

		// set each collectible to random position at start of game
		for (int i = 0; i < coins.Length; i++)
        {
            float xPos = Random.Range(minRange, maxRange);
			float zPos = Random.Range(minRange, maxRange);
            randPos.Set(xPos, yPos, zPos);
			coins[i].transform.position = randPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
