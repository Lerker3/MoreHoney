using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	//hand the spawner a copy of the enemy prefab
	public GameObject enemyPrefab;
	//have a reference to the canvas
	public Transform theCanvas;
	//which direction should this spawner send its spawnees
	public Vector2 direction;

	public void Spawn()
	{		
		//create new object
		GameObject enemy = Instantiate(enemyPrefab, this.GetComponent<Transform> ().localPosition, Quaternion.identity) as GameObject;
		//make the spawnee a child of the canvas
		enemy.transform.SetParent(theCanvas);
		//set the proper scaling after parenting to the canvas
		enemy.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
		//set the thrust
		enemy.GetComponent<Enemy> ().thrust = direction;
		//set the collider to half the width of the enemy
		enemy.GetComponent<CircleCollider2D>().radius = enemy.GetComponent<RectTransform>().rect.width/3;
	}
}
