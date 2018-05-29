using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class IVPickupStationController : PickupStationController
{
    public GameObject CooldownProgressPrefab;
    public Transform CooldownProgressIconPos;
    public List<GameObject> IVBags;
    public float AvailableBags;
    public float IVBagRespawnTime;
    private bool IVRespawnCoroutineIsRunning;
    private Transform MainCanvas;

    public override bool CanBeActionedExtended(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        return pCurrentTool == ToolName.NoTool && IsActionActive && AvailableBags > 0;
    }

    public override void OnFinishedAction(GameObject pObjectActioning)
    {
        base.OnFinishedAction(pObjectActioning);

        PickUpIVBags();
        base.OnFinishedAction(pObjectActioning);
    }

    protected override void Initialize()
    {
        base.Initialize();
        IVRespawnCoroutineIsRunning = false;
        AvailableBags = 3;
        MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
    }


    private void PickUpIVBags()
    {
        if (AvailableBags > 0)
        {
            var activeBags = IVBags.Where(a => a.activeInHierarchy);
            activeBags.FirstOrDefault().gameObject.SetActive(false);
            AvailableBags--;
            if (!IVRespawnCoroutineIsRunning)
            {
                StartCoroutine(RespawnIVBags());
            }
        }
    }

    private void SpawnIVBag()
    {
        var activeBags = IVBags.Where(a => !a.activeInHierarchy);
        activeBags.FirstOrDefault().gameObject.SetActive(true);
        AvailableBags++;
    }

    private IEnumerator RespawnIVBags()
    {
        IVRespawnCoroutineIsRunning = true;
        var respawnProgress = UISpawner.SpawnUIFromTransform(CooldownProgressPrefab, CooldownProgressIconPos, UIHierarchy.ProgressBars);
        float timer = 0;
        while (AvailableBags < 3)
        {
            timer += Time.deltaTime;
            respawnProgress.transform.GetChild(0).GetComponent<Image>().fillAmount = timer / IVBagRespawnTime;

            if (timer >= IVBagRespawnTime)
            {
                SpawnIVBag();
                timer = 0;
            }

            yield return null;
        }
        Destroy(respawnProgress);
        IVRespawnCoroutineIsRunning = false;
    }
}
