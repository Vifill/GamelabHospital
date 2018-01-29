using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkBubbleManager : MonoBehaviour 
{
    private Canvas Canvas;
    private Image TalkBubble;
    private Text BubbleText;
    private Transform Parent;
    private float TextLifetime;

    public GameObject TalkBubblePrefab;
    public float UiOffset;

	// Use this for initialization
	private void Start () 
	{
		
	}
	
	// Update is called once per frame
	private void Update () 
	{
		
	}

    public void CreateTalkBubble(Transform pParentPos, string pText, float pLifetime)
    {

    }
}
