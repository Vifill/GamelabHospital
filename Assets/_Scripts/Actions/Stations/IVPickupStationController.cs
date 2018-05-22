using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IVPickupStationController : PickupStationController
{
    public List<GameObject> IVBags;
    public float AvailableBags;
    public float IVBagRespawnTime;
    private bool IVRespawnCoroutineIsRunning;

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
        float timer = 0;
        while (AvailableBags < 3)
        {
            timer += Time.deltaTime;

            if (timer >= IVBagRespawnTime)
            {
                SpawnIVBag();
                timer = 0;
            }

            yield return null;
        }
        IVRespawnCoroutineIsRunning = false;

    }
}
