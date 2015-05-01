using UnityEngine;
using System.Collections;

//palm Linker is used on the player object to coordinate non-leap motion and detect leap presence otherwise
public class palm_Linker : MonoBehaviour {

	//used for leap detection
	public GameObject masterHand;

	//used for mouse movement
	public bool MouseControl = true;	//are we allowing mouse movement?
	private Vector3 mousePosition;		//where is the mouse
	public float moveSpeed = 0.1f;		//how fast can I move towards the mouse?

	// Update is called once per frame
	void Update () 
	{
		//if we are not using the mouse for controls and the leap does not detect any hand, delete the object
		if (masterHand == null && !MouseControl)
			Destroy (this.gameObject);

		else 
		{
			//if the leap is not activated and we can use mouse control
			//move the player towards the mouse position
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
		}
	}
}
