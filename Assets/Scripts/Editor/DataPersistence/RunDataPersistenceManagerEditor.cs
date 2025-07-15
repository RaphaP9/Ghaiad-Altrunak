using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JSONRunDataPersistenceManager))]
public class RunDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        JSONRunDataPersistenceManager runDataPersistenceManager = (JSONRunDataPersistenceManager)target;

        if(GUILayout.Button("Delete Data File"))
        {
            runDataPersistenceManager.DeleteGameData();
        }
    }
}
