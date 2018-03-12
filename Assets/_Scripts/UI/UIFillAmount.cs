using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFillAmount : MonoBehaviour{

    public Material SourceMaterial;
    private Material Material;
    private Image Image;
    private float pFillAmount;
    public float FillAmount
    {
        set
        {
            SetFillAmount(value);
            pFillAmount = value;
        }
        get { return pFillAmount; }
    }

	void Start () {
        Image = GetComponent<Image>();
        Material = new Material(SourceMaterial);
        if (Image != null)
        {
            Image.material = Material;
        }
    }
	
	void SetFillAmount(float pFill)
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        if (Image != null)
        {
            Image.material.SetFloat("_BucketFillAmount", pFill);
        }
        else
        {
            print("<color=yellow> Material not found! </color>");
        }
    }
}
