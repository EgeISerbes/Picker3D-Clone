using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        LevelManager level = (LevelManager)target;
        if(GUILayout.Button("Clear Levels"))
        {
            level.ClearData();
        }
    }   
}
