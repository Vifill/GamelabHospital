using UnityEngine;
using UnityEditor;
using System;

public class CustomShaderGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // render the default gui
        base.OnGUI(materialEditor, properties);

        Material targetMat = materialEditor.target as Material;

        // see if redify is set, and show a checkbox
        bool fluid = Array.IndexOf(targetMat.shaderKeywords, "FLUID_ON") != -1;
        EditorGUI.BeginChangeCheck();
        fluid = EditorGUILayout.Toggle("Redify material", fluid);
        if (EditorGUI.EndChangeCheck())
        {
            // enable or disable the keyword based on checkbox
            if (fluid)
                targetMat.EnableKeyword("FLUID_ON");
            else
                targetMat.DisableKeyword("FLUID_ON");
        }
    }
}