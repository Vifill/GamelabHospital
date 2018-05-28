using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public CollectableModel CollectableModel;
    public GameObject InfoscreenPopupPrefab;
    public Shader HighlightShader;

    private GameObject Infoscreen;
    private bool isActive;
    private bool isPaused;

	void Start ()
	{
	    //CollectableModel.Level = FindObjectOfType<LevelManager>().LevelConfig.LevelNumber;
	    if (PlayerPrefs.HasKey(PlayerDataController.CollectableKey))
	    {
	        CollectableModel collectableModel = JsonUtility.FromJson<Collectables>(PlayerPrefs.GetString(PlayerDataController.CollectableKey)).CollectableList.FirstOrDefault(a => a.Equals(CollectableModel));
	        if (collectableModel != null)
	        {
	            CollectableModel.IsFound = collectableModel.IsFound;
            }
	        else
	        {
	            print("collectablemodel doesnt exist");
	        }
	        
        }
	    else
	    {
	          print("No collectable key found");
	    }
	}
	
	void Update () {
	    if (Input.GetKeyDown(KeyCode.F))
	    {
            print("PlayerPrefs deleted");
	        PlayerPrefs.DeleteAll();
        }

	    if (isActive && Input.GetMouseButtonDown(0) && !isPaused)
	    {
	        if (!CollectableModel.IsFound)
	        {
	            CollectableModel.IsFound = true;
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
                if (CollectableModel.IsFound)
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

[Serializable]
public class CollectableModel
{
    [Header("REMEMBER TO APPLY THE PREFAB IN THE LEVELSELECT")]
    public int ID;
    public int Level;
    public Image Image;
    public bool IsFound;
    
    public override bool Equals(object obj)
    {
        CollectableModel referenceModel = (CollectableModel) obj;
        return ID == referenceModel.ID && Level == referenceModel.Level;
    }
}
