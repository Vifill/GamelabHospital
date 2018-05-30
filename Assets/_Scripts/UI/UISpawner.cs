using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;

public class UISpawner : MonoBehaviour
{
    [Serializable]
    public class SerializableUIDictionary : SerializableDictionaryBase<UIHierarchy, GameObject> { }

    private static List<Tuple<GameObject, Vector3>> GameObjectsAndWorldPos = new List<Tuple<GameObject, Vector3>>();
    private static List<Tuple<GameObject, Transform>> GameObjectsAndTransforms = new List<Tuple<GameObject, Transform>>();

    public SerializableUIDictionary UIDictionary;

    private static UISpawner uiSpawner;

    public static UISpawner instance
    {
        get
        {
            if (!uiSpawner)
            {
                uiSpawner = FindObjectOfType(typeof(UISpawner)) as UISpawner;

                if (!uiSpawner)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
            }

            return uiSpawner;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        ClearLists();
        foreach (var item in GameObjectsAndWorldPos)
        {
            if (item.Item1 != null)
            {
                item.Item1.transform.position = Camera.main.WorldToScreenPoint(item.Item2);
            }
        }
        foreach (var item in GameObjectsAndTransforms)
        {
            item.Item1.transform.position = Camera.main.WorldToScreenPoint(item.Item2.transform.position);
        }
    }

    private void ClearLists()
    {
        GameObjectsAndWorldPos = GameObjectsAndWorldPos.Where(a => a.Item1 != null).ToList();
        GameObjectsAndTransforms = GameObjectsAndTransforms.Where(a => a.Item1 != null && a.Item2 != null).ToList();
    }

    public static GameObject SpawnUIFromWorldPosition(GameObject pPrefab, Vector3 pWorldPosition, UIHierarchy pUIHierarchy, Vector3 pOffset = new Vector3())
    {
        var obj = SpawnUI(pPrefab, Camera.main.WorldToScreenPoint(pWorldPosition + pOffset), pUIHierarchy);
        GameObjectsAndWorldPos.Add(new Tuple<GameObject, Vector3>(obj, pWorldPosition));
        return obj;
    }

    public static GameObject SpawnUIFromTransform(GameObject pPrefab, Transform pTransformToFollow, UIHierarchy pUIHierarchy, Vector3 pOffset = new Vector3())
    {
        var obj = SpawnUI(pPrefab, Camera.main.WorldToScreenPoint(pTransformToFollow.position + pOffset), pUIHierarchy);
        GameObjectsAndTransforms.Add(new Tuple<GameObject, Transform>(obj, pTransformToFollow));
        return obj;
    }

    public static GameObject SpawnUIFromUIPosition(GameObject pPrefab, Vector3 pUIPosition, UIHierarchy pUIHierarchy)
    {
        return SpawnUI(pPrefab, pUIPosition, pUIHierarchy);
    }

    public static GameObject SpawnUIWithNoPos(GameObject pPrefab, UIHierarchy pUIHierarchy)
    {
        var parentUI = instance.UIDictionary[pUIHierarchy];

        GameObject obj = Instantiate(pPrefab, parentUI.transform, false);
        return obj;
    }

    private static GameObject SpawnUI(GameObject pPrefab, Vector3 pUIPos, UIHierarchy pUIHierarchy)
    {
        var parentUI = instance.UIDictionary[pUIHierarchy];

        GameObject obj = Instantiate(pPrefab, parentUI.transform, false);
        obj.transform.position = pUIPos;
        return obj;
    }
}

public enum UIHierarchy
{
    ProgressBars,
    PatientUI,
    UIScreens,
    StaticUI,
    LerpingPoints
}