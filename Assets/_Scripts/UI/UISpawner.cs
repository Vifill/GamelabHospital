using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;

public class UISpawner : MonoBehaviour
{
    [System.Serializable]
    public class SerializableUIDictionary : SerializableDictionaryBase<UIHierarchy, GameObject> { }

    private List<Tuple<GameObject, Vector3>> GameObjectsAndWorldPos = new List<Tuple<GameObject, Vector3>>();
    private List<Tuple<GameObject, Transform>> GameObjectsAndTransforms = new List<Tuple<GameObject, Transform>>();

    public SerializableUIDictionary UIDictionary;

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        foreach (var item in GameObjectsAndWorldPos)
        {
            item.Item1.transform.position = Camera.main.WorldToScreenPoint(item.Item2);
        }
        foreach (var item in GameObjectsAndTransforms)
        {
            item.Item1.transform.position = Camera.main.WorldToScreenPoint(item.Item2.transform.position);
        }
    }

    public GameObject SpawnUIFromWorldPosition(GameObject pPrefab, Vector3 pWorldPosition, UIHierarchy pUIHierarchy)
    {
        var obj = SpawnUI(pPrefab, Camera.main.WorldToScreenPoint(pWorldPosition), pUIHierarchy);
        GameObjectsAndWorldPos.Add(new Tuple<GameObject, Vector3>(obj, pWorldPosition));
        return obj;
    }

    public GameObject SpawnUIFromTransform(GameObject pPrefab, Transform pTransformToFollow, UIHierarchy pUIHierarchy)
    {
        var obj = SpawnUI(pPrefab, Camera.main.WorldToScreenPoint(pTransformToFollow.position), pUIHierarchy);
        GameObjectsAndTransforms.Add(new Tuple<GameObject, Transform>(obj, pTransformToFollow));
        return obj;
    }

    private GameObject SpawnUI(GameObject pPrefab, Vector3 pUIPos, UIHierarchy pUIHierarchy)
    {
        var parentUI = UIDictionary[pUIHierarchy];
        var newPosition = Camera.main.WorldToScreenPoint(pUIPos);

        GameObject obj = Instantiate(pPrefab, parentUI.transform, true);
        obj.transform.position = newPosition;
        return obj;
    }
}

public enum UIHierarchy
{
    ProgressBars,
    PatientUI,
    UIScreens
}