using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Palm_UI : MonoBehaviour {
	
	public GameObject palmPrefab;
	GameObject palmUI;
	Transform theCanvas;

	// Use this for initialization
	void Start () {
		theCanvas = GameObject.Find("Canvas/SceneObjects").GetComponent<Transform>();
		palmUI = Instantiate(palmPrefab, this.GetComponent<Transform> ().localPosition, Quaternion.identity) as GameObject;
		palmUI.GetComponent<palm_Linker>().masterHand = this.gameObject;
		palmUI.transform.SetParent(theCanvas);
		palmUI.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log("this = " + this.GetComponent<Transform>().position + "     palm = " + palmUI.GetComponent<RectTransform>().position);
		palmUI.GetComponent<RectTransform>().position = new Vector3(
			this.GetComponent<Transform>().position.x,
			this.GetComponent<Transform>().position.y, 
			theCanvas.GetComponent<Transform>().position.z);

	}
}
