using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPlacementManager : MonoBehaviour 
{
    public Sprite FilledStarSprite;
    public GameObject StarFillParticlePrefab;
    public bool Filled { get; private set; }
    public Image StarImage;

	public void FillStar()
    {
        if (StarFillParticlePrefab != null)
        {
            Instantiate(StarFillParticlePrefab, StarImage.transform);
        }

        StarImage.sprite = FilledStarSprite;
        Filled = true;
    }
}
