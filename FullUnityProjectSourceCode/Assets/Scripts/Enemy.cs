using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour 
{
	GameManager myManager;	//instance of the gameManager
	public Vector2 thrust;	//which direction to move in
	Vector3 thrust3d;		//used for 
	float t = 0;			//keep track of time

	void Start()
	{
		//make the 2d thrust into a 3d thrust since Transform.position is picky
		thrust3d = new Vector3(thrust.x, thrust.y, 0);
		//find the game manager in the object heirarchy
		myManager = GameObject.Find("Canvas").GetComponent<GameManager>();

		if (thrust.y < 0) 
		{
			GetComponent<RectTransform> ().Rotate (Vector3.back * 90);
			return;
		}
		if (thrust.y > 0) 
		{
			GetComponent<RectTransform> ().Rotate (Vector3.back * -90);
			return;
		}
		if (thrust.x < 0) 
		{
			GetComponent<RectTransform> ().localScale = new Vector3(-1,1,1);
			return;
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		//if this object is 5 seconds old, destroy it
		if((t+= Time.deltaTime) > 5f)	Destroy(this.gameObject);

		//move in a direction according to thrust
		GetComponent<Transform>().position += thrust3d * Time.deltaTime;
	}

	//called when 2d collisions occur
	void OnTriggerEnter2D(Collider2D other)
	{
		//if the object that collided is the player, subtract points and destroy thyself
		if(other.gameObject.GetComponent<palm_Linker>() != null)
		{
			myManager.IncrementScore(false, 3);
			Destroy(this.gameObject);
		}
	}
}
