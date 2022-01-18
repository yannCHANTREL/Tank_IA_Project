using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;

[CustomEditor(typeof(VirtualGrid))]
public class VirtualGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VirtualGrid virtualGrid = (VirtualGrid) target;
        GUILayout.Space(10);
        if (GUILayout.Button("Bake", GUILayout.Height(20)))
        {
            virtualGrid.CreateGrid();
        }
    }
}
