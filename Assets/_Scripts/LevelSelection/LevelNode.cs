using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNode : MonoBehaviour 
{
    public LevelConfig LevelConfig;
    public GameObject LevelSelectionUIPrefab;
    public int LevelNo;
    public Shader HighlightShader;
    public bool IsActive = false;
    public Vector3 UIOffset = new Vector3();

    private Shader StandardShader;
    private Transform MainCanvas;
    private List<Material> HighlightMaterials = new List<Material>();
    private GameObject PopUpUI;
    private LevelModel LevelMdl;
    private Vector3 UIPos;
    private MouseCursorController CursorCtrl;

    private GameObject ArrowParticleEffect;
    private GameObject SparkParticleEffect;
    private SceneLoader SceneLoader;
    private bool StartLoad;

    private void Awake()
    {
        SceneLoader = FindObjectOfType<SceneLoader>(); 
        SetParticleParent();
        CursorCtrl = FindObjectOfType<MouseCursorController>();
    }

    // Use this for initialization
    public void Initialize (LevelModel pLvlModel) 
	{
        if (pLvlModel != null)
        {
            LevelMdl = pLvlModel;
        }

        IsActive = true;
	    MainCanvas = FindObjectOfType<Canvas>().transform;
        UIPos = Camera.main.WorldToScreenPoint(transform.position);
        StandardShader = Shader.Find("Standard");
        ActivateSparkParticleEffect();
        
        var highlightable = transform.Find(Constants.Highlightable);
        foreach (var renderer in highlightable.GetComponentsInChildren<Renderer>())
        {
            HighlightMaterials.AddRange(renderer.materials);
        }

        foreach (Material mat in HighlightMaterials)
        {
            mat.color = Color.green;
        }

	    PopUpUI = Instantiate(LevelSelectionUIPrefab, MainCanvas);
	    PopUpUI.transform.position = UIPos;
	    PopUpUI.GetComponent<LevelSelectionUI>().Initialize(LevelConfig, LevelNo, LevelMdl);
    }

    private void SetParticleParent()
    {
        //ArrowParticleEffect = transform.GetChild(1)?.gameObject;
        //ArrowParticleEffect.SetActive(false);
        //SparkParticleEffect = transform.GetChild(2)?.gameObject;
        //SparkParticleEffect.SetActive(false);
    }

    internal void ActivateArrowParticleEffect()
    {
        //ArrowParticleEffect.SetActive(true);
    }

    internal void ActivateSparkParticleEffect()
    {
        //SparkParticleEffect.SetActive(true);
    }

    private void Update()
    {
        if(StartLoad)
        {
            SceneLoader.LoadScene("Level" + LevelNo);
            StartLoad = false;
        }
    }

    private void OnMouseEnter()
    {
        if (IsActive)
        {
            foreach (var mat in HighlightMaterials)
            {
                mat.shader = HighlightShader;
            }
            //var pos = Camera.main.WorldToScreenPoint(transform.position) + UIOffset;
           
            CursorCtrl.SetCursorToClickable();
        }
    }

    private void OnMouseExit()
    {
        if (IsActive)
        {
            foreach (var mat in HighlightMaterials)
            {
                mat.shader = StandardShader;
            }
            CursorCtrl.SetCursorToIdle();
        }
    }

    private void OnMouseDown()
    {
        if (IsActive)
        {
            CursorCtrl.OnClickDown();
            if (LevelNo != 0)
            {
                //StartLoad = true;
                SceneLoader.LoadScene("Level" + LevelNo);
            }
            else
            {
                SceneLoader.LoadScene("TutorialLevel");
            }
        }
    }

    private void OnMouseUp()
    {
        if (IsActive)
        {
            CursorCtrl.OnClickUp();
        }
    }
}
