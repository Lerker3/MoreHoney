using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HoneyFill : MonoBehaviour {

	//core data types for hex
	Image honey;
	float amntFilledCurrent = 0f;	//how much honey is in cell (range 0-1)
	float amntFilledInitial;		//how much hone is in cell when queen enters
	bool canFill = true;			//can player add honey to cell?
	bool draining = false;			//used to tell if the cell is being drained

	//for fill transition
 	float timeSpan = 0.5f;		//how long should it take to drain cell
	float t = 0;						//keep track of time

	//game state stuff
	bool gameActive = false;

	// Use this for initialization
	void Start () 
	{
		//get image reference of current object
		honey = GetComponent<Image> ();
		//set the fill amount to zero
		honey.fillAmount = amntFilledCurrent;
	}

	//starts the game
	public void StartGame()
	{
		//set the game to active
		gameActive = true;
		//set the fill amount to zero
		honey.fillAmount = 0;
	}

	//starts the game
	public void EndGame()
	{
		//set the game to active
		gameActive = false;

		//set the fill amount to zero
		amntFilledCurrent = 0;
		honey.fillAmount = 0;
	}

	//Drain the cell of honey
	public IEnumerator Drain()
	{
		if(gameActive)
		{
			while((t += Time.deltaTime) < timeSpan * amntFilledInitial)
			{
				//take whatever this cell is filled with and drain it over timespan
				amntFilledCurrent = amntFilledInitial * (1 - (t / (timeSpan * amntFilledInitial)));
				honey.fillAmount = amntFilledCurrent;
				yield return null;
			}
			
			//once you're done reset values
			honey.fillAmount = 0;
			t = 0;
		}
		
		canFill = true;
		draining = false;
		yield return 0;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//drain cell when queen is ready
		if (other.gameObject.GetComponent<QueenBeeController> () != null)
		{
			//get the current amount filled for smooth transition data
			amntFilledInitial = amntFilledCurrent;

			//make the player unable to fill
			canFill = false;

			//let the queen know how much she should fill up and for how long
			other.gameObject.GetComponent<QueenBeeController>().cellFillAmount = amntFilledCurrent;
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		//fill up the hex with honey when player is in contact
		if (gameActive && canFill && other.gameObject.GetComponent<palm_Linker> () != null) 
		{
			//Add honey at a rate of Time.delta/2
			//If the cell is over a value of 1, set it back to 1
			if((amntFilledCurrent += (Time.deltaTime/2)) > 1) amntFilledCurrent=1;
			//change the image to fit the fill amount
			GetComponent<Image> ().fillAmount = amntFilledCurrent;
		}

		if (other.gameObject.GetComponent<QueenBeeController> () != null &&
		    other.gameObject.GetComponent<QueenBeeController> ().shouldDrain() &&
		    !draining)
		{
			StartCoroutine(Drain ());
			draining = true;
		}
	}

	//make sure the cell can be filled after the queen leaves the cell
	void OnTriggerExit2D(Collider2D other)
	{
		//check that the object leaving is the queen
		if (other.gameObject.GetComponent<QueenBeeController> () != null)
		{
			//make the player able to fill
			canFill = true;
		}
	}
}
