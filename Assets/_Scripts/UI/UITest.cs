using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour 
{
    public GameObject Prefab;
    public Vector3 Offset = new Vector3(0, 20, 0);
    private GameObject Object;

	// Use this for initialization
	private void Start () 
	{
        Object =  Instantiate(Prefab);
        var playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Object.transform.position = Camera.main.ScreenToWorldPoint(playerPos + Offset);
        Object.transform.rotation = Camera.main.transform.rotation;
	}
	
	// Update is called once per frame
	private void Update () 
	{
        var playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Object.transform.position = Camera.main.ScreenToWorldPoint(playerPos + Offset);
    }
}
