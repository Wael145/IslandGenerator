using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(DisplayTexture))]
public class DisplayTextureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DisplayTexture display = (DisplayTexture)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            display.GenerateMap();
        }
    }
}
