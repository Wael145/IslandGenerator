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
        // Ajoute un bouton "Update Placement" � l'Inspector
        DrawDefaultInspector();
        // Si le bouton est cliqu�, appelle la m�thode
        if (GUILayout.Button("Update Placement"))
        {
            displayMap.UpdatePlacement();
        }
    }
}

