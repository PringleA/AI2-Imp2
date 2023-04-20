using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverHandler : MonoBehaviour
{
	GameObject[] cover;
	Vector3 randPos;
    float[] xPositions;
	float[] zPositions;
	float minPos = -20.0f;
	float gridSpacing = 10.0f;
	int totalSquares = 5;

	// Start is called before the first frame update
	void Start()
    {
		cover = GameObject.FindGameObjectsWithTag("Cover");
		GetXZPositions();
		PlaceCover();
    }

	void GetXZPositions()
	{
		float xPos = 0;
		float zPos = 0;
		// creating 5 different positions for a total of 25 unique potential positions
		xPositions = new float[totalSquares];
		zPositions = new float[totalSquares];

		// data driven method of retrieving every potential position for cover
		for (int i = 0; i < totalSquares; i++)
		{
			xPos = minPos + (i * gridSpacing);
			for (int k = 0; k < totalSquares; k++)
			{
				zPos = minPos + (k * gridSpacing);
				xPositions[i] = xPos;
				zPositions[k] = zPos;
			}
		}
	}

	void PlaceCover()
	{
		int randXPos;
		int randZPos;
		float yPos = 0.19f;
		bool uniqueCover = false;
		// iterating through cover and giving spots
		int totalCover = cover.Length;

		randXPos = Random.Range(1, totalSquares - 1);
		randZPos = Random.Range(1, totalSquares - 1);

		Vector3 testCoverPos = new Vector3(xPositions[randXPos], yPos, zPositions[randZPos]);
		cover[0].transform.position = testCoverPos;

		for (int i = 1; i < totalCover; i++)
		{
			uniqueCover = false;
			// while cover is not unique
			while (!uniqueCover)
			{
				// get new randomisation
				randXPos = Random.Range(1, totalSquares - 1);
				randZPos = Random.Range(1, totalSquares - 1);
				testCoverPos = new Vector3(xPositions[randXPos], yPos, zPositions[randZPos]);

				//check it is not the same as another piece of cover
				uniqueCover = CheckNewCoverPos(i, testCoverPos);
			}
			//if while loop has ended, place cover at unique pos
			cover[i].transform.position = testCoverPos;
		}
	}

	bool CheckNewCoverPos(int currentIteration, Vector3 testPos)
	{
		/* set as true initially, if it equals one piece of cover 
		 * it will be set and remain false for rest of function */
		bool isUnique = true;
		for (int i = 0; i < currentIteration; i++)
		{
			if (cover[i].transform.position.Equals(testPos))
			{
				isUnique = false;
			}
		}
		return isUnique;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
