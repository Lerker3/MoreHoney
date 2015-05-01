using UnityEngine;
using System.Collections;

public class EnemySpawnController: MonoBehaviour 
{
	float t = 0;						//keep track of time
	public float NextSpawn = .5f;		//how often should a cannon be fired
	public EnemySpawn[] spawners;		//list cannons that can be fired
	bool gameActive = false;			//game status

	// Update is called once per frame
	void Update () 
	{
		//if the game is active and it is time to fire off a shot, fire away
		if (gameActive && NextSpawn < (t += Time.deltaTime)) 
		{
			//select a random cannon to fire
			spawners [Random.Range (0, spawners.Length)].Spawn();
			//reset the timer to zero
			t = 0;
		}
	}

	public void StartGame()
	{
		gameActive = true;
	}

	public void EndGame()
	{
		gameActive = false;
	}
}
