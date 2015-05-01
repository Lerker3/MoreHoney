using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QueenBeeController : MonoBehaviour 
{
	public GameManager myManager;			//instance of the gameManager
	public List<RectTransform> destinations;//array of gameobjects that the Queen can go to
	float timeSpan = 0.5f;					//how long should it take to get from A to B
	float t = 0;							//keep track of how far along in a move we are
	float t2 = 0;							//keep track of how far along in a honey pick up we are
	RectTransform currRectTran;				//RectTran of the Queen
	float initialQueenScale;				//find the value of the queens scale when the game starts
	float filledQueenScale = 0.85f;			//how big should the queen get when filled to max
	float amntQueenFilled;					//what percent of a cell's honey is the queen holding (0 to 1)
	Vector2 DifferencePosition;				//how far along you should be in your movement
	Vector2 DestinationPos;					//where you're going
	Vector2 SourcePos;						//where you're coming from
	int destPos = 0;						//index of which array element in destinations[] your going to
	int score = 0;							//take a guess
	bool gameActive = false;				//for the game state
	bool scaleChange = false;				//let the cells know when the queen is loading/unloading
	public float cellFillAmount = 0;
	
	// Use this for initialization
	void Start () 
	{
		currRectTran = this.GetComponent<RectTransform> ();
		initialQueenScale = currRectTran.localScale.x;
	}

	//finds a new cell fo the queen to go to
	public void NewDest()
	{ 

		//curr position stores where the Queen is currently
		int currPos = destPos;

		//if the Queen is in the middle go somewhere else, else, go to middle
		if (destPos == 0) 
		{
			//let the manager know that it should increment the score
			myManager.IncrementScore(true, score);
			score = 0;
			destPos = Random.Range (1, destinations.Count);
		}
		else 
		{
			destPos = 0;
		}

		//Set the source position and destination position for smooth movement 
		DestinationPos = new Vector2(destinations[destPos].localPosition.x, destinations[destPos].localPosition.y);
		SourcePos = new Vector2(destinations[currPos].localPosition.x, destinations[currPos].localPosition.y);

		//reset t for the new move
		t = 0;
	}

	public IEnumerator Load()
	{
		//figure out the points alloted for the cell.
		//magic numbers are percentages between 0 and 1
		if(cellFillAmount >= .05 && cellFillAmount < .35)			score += 1;
		else if (cellFillAmount >= .35 && cellFillAmount < .85) 	score += 2;
		else if (cellFillAmount >= .85)								score += 4;

		//keep track of the amount the queen should fill up
		float DifferenceScale = 0;
		float newScale = 0;
		amntQueenFilled = cellFillAmount;

		t2 = 0;		//reset the time for a new smooth transition

		//if the cell isn't empty, do this stuff
		if(cellFillAmount >= .05)
		{
			//tell the cell currently in to drain
			scaleChange = true;

			//if the timespan hasnt been reached then find out at what stage in the move the Queen should be
			while ((t2 += Time.deltaTime) < cellFillAmount * timeSpan) 
			{
				//calculate what the Queen's scale should be should be with a sin curve
				DifferenceScale = ((filledQueenScale - initialQueenScale) * cellFillAmount) * Mathf.Sin (90 * Mathf.Deg2Rad * t2 / (timeSpan *  cellFillAmount));	
				//add the difference just calculated to the source position
				newScale = initialQueenScale + DifferenceScale;
				currRectTran.localScale = new Vector2(newScale,newScale);

				yield return null;
			} 
		}
		//if it is basically empty, set the scale to the default initial
		else 	newScale = initialQueenScale;

		//if the time span has been reached set the position exactly and get a new destination
		currRectTran.localScale = new Vector2(newScale,newScale);

		//go to the next state in the queen's cycle
		scaleChange = false;
		NewDest ();
		if (gameActive) StartCoroutine(Move ());
		yield return 0;
	}

	public IEnumerator Unload()
	{
		//keep track of the amount the queen should expel
		float DifferenceScale = 0;
		float newScale = 0;
		
		t2 = 0;		//reset the time for a new smooth transition
		
		//if the timespan hasnt been reached then find out at what stage in the move the Queen should be
		while (gameActive && (t2 += Time.deltaTime) < amntQueenFilled * timeSpan) 
		{
			//calculate what the Queen's scale should be should be with a sin curve
			DifferenceScale = ((filledQueenScale - initialQueenScale)* amntQueenFilled) * Mathf.Sin (90 * Mathf.Deg2Rad * t2 / (timeSpan *  amntQueenFilled));	
			//add the difference just calculated to the source position
			newScale = initialQueenScale + ((filledQueenScale - initialQueenScale)* amntQueenFilled) - DifferenceScale;
			currRectTran.localScale = new Vector2(newScale,newScale);
			
			yield return null;
		} 

		//if the time span has been reached set the scale exactly to the goal
		currRectTran.localScale = new Vector2(initialQueenScale,initialQueenScale);

		//go to the next state in the queen's cycle
		NewDest ();
		if (gameActive)	StartCoroutine(Move ());
		yield return 0;
	}


	public IEnumerator Move() 
	{
		//if the timespan hasnt been reached then find out at what stage in the move the Queen should be
		while ((t += Time.deltaTime) < timeSpan) 
		{
			//calculate where the Queen should be with a sin curve
			DifferencePosition = (DestinationPos - SourcePos) * Mathf.Sin ((Mathf.Deg2Rad * t / timeSpan * 90));	
			//add the difference just calculated to the source position
			currRectTran.localPosition = SourcePos + DifferencePosition;
			//if not done, yield
			yield return null;
		} 

		//if the time span has been reached set the position exactly and get a new destination
		currRectTran.localPosition = DestinationPos;

		//manage what happens next in the cycle
		if (gameActive && destPos == 0)	StartCoroutine(Unload());
		else if (gameActive)	StartCoroutine(Load());
		yield return 0;

	}


	//activates some start conditions for the queen
	public void StartGame()
	{
		//queen should be default scale
		amntQueenFilled = 0;
		currRectTran.localScale = new Vector2(initialQueenScale,initialQueenScale);

		//set the game active
		gameActive = true;
		//queen should be in the center cell
		destPos = 0;
		currRectTran.localPosition = new Vector2(destinations[destPos].localPosition.x, destinations[destPos].localPosition.y);
		//but also find a place for her to go ASAP
		NewDest ();
		StartCoroutine(Move ());
	}
		
	public void EndGame()
	{
		//set the game active
		gameActive = false;

		//set the queen to normal scale
		//currRectTran.localScale = new Vector2(initialQueenScale,initialQueenScale);

		//queen should be in the center cell
		//destPos = 0;
		//currRectTran.localPosition = new Vector2(destinations[destPos].localPosition.x, destinations[destPos].localPosition.y);
	}

	//quick function letting the cell know when to start draining
	public bool shouldDrain()
	{
		return scaleChange;
	}

}
