using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorController : MonoBehaviour 
{
    public Texture2D CursorIdle;
    public Texture2D CursorHover;
    public Texture2D CursorClick;
    public AudioSource MouseClickAudioSource;
    public AudioClip MouseClickDownSound;
    public AudioClip MouseClickUpSound;
    public bool IsEnabledInLevel;

    private Texture2D PreviousCursor;
	// Use this for initialization
	private void Start () 
	{
        var orderly = FindObjectOfType<OrderlyController>();
        IsEnabledInLevel = orderly != null;

        if (GameController.InMenuScreen || IsEnabledInLevel)
        {
            Cursor.SetCursor(CursorIdle, Vector2.zero, CursorMode.Auto);
            PreviousCursor = CursorIdle;
        }
    }

    private void Update()
    {
        Cursor.visible = IsEnabledInLevel;
    }

    public void SetCursorToIdle()
    {
        if (IsEnabledInLevel)
        {
            Cursor.SetCursor(CursorIdle, Vector2.zero, CursorMode.Auto);
            PreviousCursor = CursorIdle;
        }
    }

    public void SetCursorToClickable()
    {
        if (IsEnabledInLevel)
        {
            Cursor.SetCursor(CursorHover, Vector2.zero, CursorMode.Auto);
            PreviousCursor = CursorHover;
        }
    }

    public void OnClickDown()
    {
        if (IsEnabledInLevel)
        {
            Cursor.SetCursor(CursorClick, Vector2.zero, CursorMode.Auto);
            MouseClickAudioSource.PlayOneShot(MouseClickDownSound);
        }
    }

    public void OnClickUp()
    {
        if (IsEnabledInLevel)
        {
            Cursor.SetCursor(PreviousCursor, Vector2.zero, CursorMode.Auto);
            MouseClickAudioSource.PlayOneShot(MouseClickUpSound);
        }
    }
}
