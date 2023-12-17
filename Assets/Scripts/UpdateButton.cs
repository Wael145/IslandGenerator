using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
[CustomEditor(typeof(DisplayMap))]
public class UpdateButton : Editor
{
    public override void OnInspectorGUI()
    {
        DisplayMap displayMap = (DisplayMap)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Update Placement"))
        {
            displayMap.UpdatePlacement();
        }
    }
}

