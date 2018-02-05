using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreenUIManager : MonoBehaviour 
{
    public Button CloseButton;
    public Button NextButton;
    public Button BackButton;
    public Image InfoImage;
    public List<Sprite> Images;

    private int CurrentImageIndex = 0;

	// Use this for initialization
	private void Start () 
	{
        var gc = FindObjectOfType<GameController>();
        CloseButton.onClick.AddListener(gc.ResumeGame);

        InfoImage.sprite = Images[0];
        if (Images.Count > 1)
        {
            CloseButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(true);
            BackButton.gameObject.SetActive(false);
        }
        else
        {
            CloseButton.gameObject.SetActive(true);
            NextButton.gameObject.SetActive(false);
            BackButton.gameObject.SetActive(false);
        }
	}

    public void NextBtn ()
    {
        CurrentImageIndex++;
        InfoImage.sprite = Images[CurrentImageIndex];

        if (CurrentImageIndex + 1 == Images.Count)
        {
            NextButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(true);
        }
        else
        {
            NextButton.gameObject.SetActive(true);
        }

        BackButton.gameObject.SetActive(true);
    }

    public void BackBtn ()
    {
        CurrentImageIndex--;
        InfoImage.sprite = Images[CurrentImageIndex];

        if (CurrentImageIndex == 0)
        {
            BackButton.gameObject.SetActive(false);
        }
        if (CurrentImageIndex + 1 != Images.Count)
        {
            CloseButton.gameObject.SetActive(false);
            NextButton.gameObject.SetActive(true);
        }
    }
}
