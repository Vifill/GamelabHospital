using System.Collections;
using System.Collections.Generic;
using Assets._Scripts.Utilities;
using UnityEngine;

public class HoverOverInfoscreens : MonoBehaviour
{
    public GameObject InfoscreenPopupPrefab;
    public Shader HighlightShader;
    public bool isFound;
    public int index;

    private GameObject Infoscreen;
    private bool isActive;
    private bool isPaused;
    private bool hasBeenActivated;
	void Start () {
		
	}
	
	void Update () {
	    if (isActive && Input.GetMouseButtonDown(0) && !isPaused)
	    {
	        if (!hasBeenActivated)
	        {
	            hasBeenActivated = true;
	        }
            FindObjectOfType<GameController>().PauseGame(InfoscreenPopupPrefab);
            isPaused = true;
        }
        else if (Input.GetMouseButtonDown(0) && isPaused)
	    {
	        Destroy(Infoscreen);
            FindObjectOfType<GameController>().ResumeGame();
            isPaused = false;
	    }
    }

    void OnMouseEnter()
    {
        SetHighlight();
        isActive = true;
    }

    void OnMouseExit()
    {
        RemoveHighlight();
        isActive = false;
    }

    void SetHighlight()
    {
        var renderers =  transform.Find("Highlightable").GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            List<Material> mats = new List<Material>();

            foreach (Renderer rend in renderers)
            {
                //rend.material.shader = pHighlightShader;
                //dont highlight particlesystems
                if (rend.GetComponent<ParticleSystem>() == null)
                {
                    mats.AddRange(rend.materials);
                }

            }

            foreach (Material mat in mats)
            {

                mat.shader = HighlightShader;
                if (hasBeenActivated)
                {
                    //mat.SetColor("_OutlineColor", Color.grey * new Color(0.67f, 1f, 0.184f));
                    mat.SetColor("_OutlineColor", Color.grey * Constants.Colors.GetColor("#DAA520"));
                }
                else
                {
                    //mat.SetColor("_OutlineColor", new Color(0.67f, 1f, 0.184f));
                    mat.SetColor("_OutlineColor", Constants.Colors.GetColor("#DAA520"));
                }
            }
        }
    }

    void RemoveHighlight()
    {
        if (gameObject == null)
        {
            return;
        }

        var renderers = transform.Find("Highlightable").GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            List<Material> mats = new List<Material>();

            foreach (Renderer rend in renderers)
            {
                //dont highlight particlesystems
                if (rend.GetComponent<ParticleSystem>() == null)
                {
                    mats.AddRange(rend.materials);
                }
                //rend.material.shader = pHighlightShader;

            }

            foreach (Material mat in mats)
            {
                mat.shader = Shader.Find("Standard");
            }
        }
    }
}
