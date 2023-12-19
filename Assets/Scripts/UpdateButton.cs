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
        // Ajoute un bouton "Update Placement" à l'Inspector
        DrawDefaultInspector();
        // Si le bouton est cliqué, appelle la méthode
        if (GUILayout.Button("Update Placement"))
        {
            displayMap.UpdatePlacement();
        }
    }
}

