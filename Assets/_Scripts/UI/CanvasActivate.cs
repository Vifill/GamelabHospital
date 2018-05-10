using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasActivate : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    gameObject.SetActive(true);
	}
}
