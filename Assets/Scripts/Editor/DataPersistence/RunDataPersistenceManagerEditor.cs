using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RunJSONDataManager))]
public class RunDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RunJSONDataManager runDataPersistenceManager = (RunJSONDataManager)target;

        if(GUILayout.Button("Delete Data File"))
        {
            runDataPersistenceManager.DeleteGameData();
        }
    }
}
