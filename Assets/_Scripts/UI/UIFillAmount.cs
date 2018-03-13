using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFillAmount : MonoBehaviour{

    public Material SourceMaterial;
    private Material Material;
    private Image Image;
    private float pFillAmount = 0;
    private float targetFillAmount;
    public float FillAmount
    {
        set
        {
            targetFillAmount = value;
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
    
    void Update()
    {
        LogaritmicTo();
    }

    void LogaritmicTo()
    {
        pFillAmount = Mathf.Lerp(pFillAmount, targetFillAmount, Time.deltaTime * 2);
        SetFillAmount(pFillAmount);
    }

    void LinearTo()
    {
        //if (Mathf.Abs(pFillAmount - targetFillAmount) > .01)
        //{
        //    pFillAmount += Mathf.Sign(targetFillAmount - pFillAmount) * Time.deltaTime * LinearSpeed;
        //    SetFillAmount(pFillAmount);
        //}
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
