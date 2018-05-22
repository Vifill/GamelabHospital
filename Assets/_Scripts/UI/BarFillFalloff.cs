using System.Collections;
using System.Collections.Generic;
using Assets._Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BarFillFalloff : MonoBehaviour {

    private Image Image;
    private Image OverlayImage;

    private Color OverlayImageColor;
    private Color GainColor;
    private float Timer;
    public bool IsGaining;

    public void Initialize ()
    {
        GainColor = Constants.Colors.Green;
        Image = GetComponent<Image>();
        OverlayImage = Instantiate(Image, Image.transform.position, Image.transform.rotation, Image.transform.parent);
        OverlayImage.transform.SetSiblingIndex(Image.transform.GetSiblingIndex());
        Image.transform.SetParent(OverlayImage.transform);
        Destroy(OverlayImage.GetComponent<BarFillFalloff>());

        OverlayImage.color = Constants.Colors.Red;
        OverlayImageColor = OverlayImage.color;
        OverlayImageColor.a = 1;
        OverlayImage.color = OverlayImage.color;
        IsGaining = false;
	}

	void LateUpdate ()
    {
        if (IsGaining || OverlayImage == null)
        {
            return;
        }
        if (Mathf.Abs(Image.fillAmount - OverlayImage.fillAmount) < 0.05)
        {
            OverlayImage.fillAmount = Mathf.MoveTowards(OverlayImage.fillAmount, Image.fillAmount, Time.deltaTime / 2);

            if (OverlayImage.color.a != 0)
            {
                Timer = 0;
                OverlayImageColor.a = 0;
                OverlayImage.color = OverlayImageColor;
            }
        }
        else
        {
            if (OverlayImage.color.a != 1)
            {
                OverlayImageColor.a = 1;
                OverlayImage.color = OverlayImageColor;
            }
        }

        if (OverlayImage.color.a == 1)
        {
            Timer += Time.deltaTime;
            if (Timer >= .75f)
            {
                OverlayImage.fillAmount = Mathf.MoveTowards(OverlayImage.fillAmount, Image.fillAmount, Time.deltaTime/2);
            }
        }
	}
    
    public void VisualGain(float pFillAmount)
    {
        if (OverlayImage == null)
        {
            return;
        }
        IsGaining = true;
        OverlayImage.color = Constants.Colors.Green;
        OverlayImage.fillAmount = pFillAmount;
        StartCoroutine(GainCoroutine(pFillAmount));
    }
  
    private IEnumerator GainCoroutine (float pFillAmount)
    {
        yield return new WaitForSeconds(.75f);

        while (Image.fillAmount < pFillAmount)
	    {
            Image.fillAmount = Mathf.MoveTowards(Image.fillAmount, pFillAmount, Time.deltaTime/2);
            yield return null;
	    }
        IsGaining = false;
    }
}