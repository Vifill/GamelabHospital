using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UISpawner))]
public class CustomUISpawner : Editor 
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        UISpawner uiSpawner = (UISpawner)target;
            
        foreach (var enumName in Enum.GetNames(typeof(UIHierarchy)))
        {
            if (uiSpawner.UIDictionary.ContainsKey((UIHierarchy) Enum.Parse(typeof(UIHierarchy), enumName)))
            {
                uiSpawner.UIDictionary[(UIHierarchy)Enum.Parse(typeof(UIHierarchy), enumName)] = (GameObject)EditorGUILayout.ObjectField(enumName, uiSpawner.UIDictionary[(UIHierarchy)Enum.Parse(typeof(UIHierarchy), enumName)], typeof(GameObject), true);
            }
            else
            {
                uiSpawner.UIDictionary[(UIHierarchy)Enum.Parse(typeof(UIHierarchy), enumName)] = (GameObject)EditorGUILayout.ObjectField(enumName, null, typeof(GameObject), true);
            }
        }
    }
}
