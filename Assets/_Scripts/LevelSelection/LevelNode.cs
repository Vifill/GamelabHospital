using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNode : MonoBehaviour
{
    public ParticleSystem HoverOverParticleSystem;
    public ParticleSystem HoverOverParticleSystem2;
    public Transform UiTransform;
    public LevelConfig LevelConfig;
    public GameObject LevelSelectionUIPrefab;
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
    private float EmissionPerSecond;
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
	    if (HoverOverParticleSystem != null)
	    {
	        ParticleSystem.EmissionModule emission = HoverOverParticleSystem.emission;
	        emission.enabled = false;
        }

        if (HoverOverParticleSystem2 != null)
        {
            ParticleSystem.EmissionModule emission = HoverOverParticleSystem2.emission;
            emission.enabled = false;
        }


        if (pLvlModel != null)
        {
            LevelMdl = pLvlModel;
        }

        IsActive = true;
	    MainCanvas = FindObjectOfType<Canvas>().transform;
        UIPos = Camera.main.WorldToScreenPoint(UiTransform.position);
        StandardShader = Shader.Find("Standard");
        ActivateSparkParticleEffect();
        
        var highlightable = transform.Find(Constants.Highlightable);
        foreach (var renderer in highlightable.GetComponentsInChildren<Renderer>())
        {
            HighlightMaterials.AddRange(renderer.materials);
        }

        //foreach (Material mat in HighlightMaterials)
        //{
        //    mat.color = Color.green;
        //}

	    PopUpUI = Instantiate(LevelSelectionUIPrefab, MainCanvas);
	    PopUpUI.transform.position = UIPos;
	    PopUpUI.GetComponent<LevelSelectionUI>().Initialize(LevelConfig, LevelConfig.LevelNumber, LevelMdl);
    }

    private void StartParticleEmission()
    {
        ParticleSystem.EmissionModule emission = HoverOverParticleSystem.emission;
        emission.enabled = true;

        ParticleSystem.EmissionModule emission2 = HoverOverParticleSystem2.emission;
        emission2.enabled = true;
    }

    private void StopParticleEmission()
    {
        ParticleSystem.EmissionModule emission = HoverOverParticleSystem.emission;
        emission.enabled = false;

        ParticleSystem.EmissionModule emission2 = HoverOverParticleSystem2.emission;
        emission2.enabled = false;
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
            SceneLoader.LoadScene("Level" + LevelConfig.LevelNumber);
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

            if (HoverOverParticleSystem != null)
            {
                StartParticleEmission();
            }
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

            if (HoverOverParticleSystem != null)
            {
                StopParticleEmission();
            }

            CursorCtrl.SetCursorToIdle();
        }
    }

    private void OnMouseDown()
    {
        if (IsActive)
        {
            CursorCtrl.OnClickDown();
            if (LevelConfig.LevelNumber != 0)
            {
                //StartLoad = true;
                //print(SceneManager.GetSceneAt());
                
                SceneLoader.LoadScene("Level" + LevelConfig.LevelNumber);
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
