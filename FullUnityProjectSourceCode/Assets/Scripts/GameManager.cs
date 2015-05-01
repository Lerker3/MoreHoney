using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public QueenBeeController myQueen;				//handle on the queen
	public ScreenZoom myZoom;						//handle on the zoom script
	public HoneyFill[] myHoney;						//handle on the cells
	public EnemySpawnController mySpawner;			//handle on the enemy spawner
	public Text myScore;							//handle on the score text object
	public GameObject mainMenu;						//handle on the main menu
	public GameObject winMenu;						//handle on winMenu
	private int score = 0;							//take a guess
	bool gameActive = false;
	int time = 0;
	float t = 0;

	void Update()
	{

		if(gameActive && (t+=Time.deltaTime) > 1.0f)
		{
			t-=1.0f;
			time++;
			IncrementScore(true,0);
		}
	}

	public void Toggle_MainMenu(bool Open)
	{
		Image[] allChildren = mainMenu.GetComponentsInChildren<Image>();
		foreach (Image child in allChildren) {
			child.enabled = Open;
		}
		
		Text[] Txt_allChildren = mainMenu.GetComponentsInChildren<Text>();
		foreach (Text child in Txt_allChildren) {
			child.enabled = Open;
		}
	}

	public void Toggle_WinMenu(bool Open)
	{
		Image[] allChildren = winMenu.GetComponentsInChildren<Image>();
		foreach (Image child in allChildren) {
			child.enabled = Open;
		}
		
		Text[] Txt_allChildren = winMenu.GetComponentsInChildren<Text>();
		foreach (Text child in Txt_allChildren) {
			child.enabled = Open;
		}
	}

	public void InitializeGame()
	{
		Toggle_MainMenu (false);
		Toggle_WinMenu(false);
		myZoom.StartGame();
		time = 0;
		t= 0;
		score = 0;
		myScore.text = "Time: " + time + "\nScore: " + score.ToString();
	}

	public void StartGame()
	{
		for(int i = 0;i < myHoney.Length;i++)	myHoney[i].StartGame ();
		myQueen.StartGame ();
		mySpawner.StartGame();
		gameActive = true;
	}

	public void EndGame()
	{
		Toggle_WinMenu(true);
		myZoom.EndGame();
		for(int i = 0;i < myHoney.Length;i++)	myHoney[i].EndGame ();
		myQueen.EndGame ();
		mySpawner.EndGame();
		score = 0;
		gameActive = false;
		myScore.text = "Time: " + time + "\nScore: 50";
	}

	//if addPoint is true add numPoints, if not, subtract numPoints
	public void IncrementScore(bool addPoint, int numPoints)
	{
		if(addPoint) score+=numPoints;
		else if((score-=numPoints) < 0) score = 0;
		myScore.text = "Time: " + time + "\nScore: " + score.ToString();

		if(score >= 50)	EndGame();
	}
}
