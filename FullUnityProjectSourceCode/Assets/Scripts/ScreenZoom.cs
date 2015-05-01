using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//this class provides a zoom effect
public class ScreenZoom : MonoBehaviour 
{
	//for game state management
	public GameManager gameController;

	//core data types for hex
	RectTransform zoomObject;		//what we are trying to fiddle with
	float finalScale = 2.3f;		//how much should I zoom in?
	float finalTop = 100f;			//how much should I move?
	bool transition = false;			//should I be transitioning?
	bool zoomIn = true;				//should I zoom in, or out?

	//for fill transition
	float timeSpan = 2.0f;		//how long should it take to drain cell
	float t = 0;						//keep track of time
	
	// Use this for initialization
	void Start () 
	{
		//get image reference of current object
		zoomObject = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if in the process of zooming in/out
		if (transition) {

			//if zooming in
			if (zoomIn) {
				if ((t += Time.deltaTime) < timeSpan) {
					//set the new position and scale in here
					zoomObject.localPosition = new Vector3 (0, finalTop * Mathf.Sin ((Mathf.Deg2Rad * t / timeSpan * 90)), 0);	
					//set the new position and scale in here
					zoomObject.localScale = new Vector3 (1 + ((finalScale - 1) * Mathf.Sin ((Mathf.Deg2Rad * t / timeSpan * 90))), 
			                                    1 + ((finalScale - 1) * Mathf.Sin ((Mathf.Deg2Rad * t / timeSpan * 90))), 1);
				} else if (t >= timeSpan) {
					//set the zoomObject to its final scale
					zoomObject.localScale = new Vector3 (finalScale, finalScale, 1);
					////et it to the righ position too
					zoomObject.localPosition = new Vector3 (0, finalTop, 0);
					//once your done reset values and flip the zoomIn
					zoomIn = false;
					transition = false;
					t = 0;
					//start the game for the rest of the components once you've finished zooming in
					gameController.StartGame();
				}
			}

			//if zooming out
			if (!zoomIn) {
				if ((t += Time.deltaTime) < timeSpan) {
					//set the new position and scale in here
					zoomObject.localPosition = new Vector3 (0, finalTop - (finalTop * t / timeSpan), 0);	
					//set the new position and scale in here
					zoomObject.localScale = new Vector3 (finalScale - ((finalScale - 1) * t / timeSpan), 
				                                     finalScale - ((finalScale - 1) * t / timeSpan), 1);
				} else if (t >= timeSpan) {
					//set the zoomObject to its final scale
					zoomObject.localScale = new Vector3 (1, 1, 1);
					////et it to the righ position too
					zoomObject.localPosition = new Vector3 (0, 0, 0);
					//once your done reset values and flip the zoomIn
					zoomIn = false;
					transition = false;
					t = 0;
				}
			}
		}
	}

	public void StartGame()
	{
		zoomIn = true;
		transition = true;
	}

	public void EndGame()
	{
		zoomIn = false;
		transition = true;
	}
}
