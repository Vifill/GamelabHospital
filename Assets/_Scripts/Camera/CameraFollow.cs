using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	[SerializeField]
	private Vector2 CameraCage = new Vector2();
	[SerializeField]
	private float Speed;

	private GameObject[] Players;

	void Start () {
		Players = FindObjectsOfType ("ActionableActioner");
	}

	void LateUpdate () {
		
	}

	void Move(Vector3 direction){
		
	}

	void FindMove(){
		var PlayerSquare = new List <Vector2> (2); //left corner 0, right corner 1
		for (int i = 0; i < Players.Length; i++) {
			
		}
	}
}
