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

    private Texture2D PreviousCursor;
	// Use this for initialization
	private void Start () 
	{
        Cursor.SetCursor(CursorIdle, Vector2.zero, CursorMode.Auto);
        PreviousCursor = CursorIdle;
    }

    public void SetCursorToIdle()
    {
        Cursor.SetCursor(CursorIdle, Vector2.zero, CursorMode.Auto);
        PreviousCursor = CursorIdle;
    }

    public void SetCursorToClickable()
    {
        Cursor.SetCursor(CursorHover, Vector2.zero, CursorMode.Auto);
        PreviousCursor = CursorHover;
    }

    public void OnClickDown()
    {
        Cursor.SetCursor(CursorClick, Vector2.zero, CursorMode.Auto);
        MouseClickAudioSource.PlayOneShot(MouseClickDownSound);
    }

    public void OnClickUp()
    {
        Cursor.SetCursor(PreviousCursor, Vector2.zero, CursorMode.Auto);
        MouseClickAudioSource.PlayOneShot(MouseClickUpSound);
    }
}
