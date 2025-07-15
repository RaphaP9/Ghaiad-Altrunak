using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JSONPerpetualDataPersistenceManager))]
public class PerpetualDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        JSONPerpetualDataPersistenceManager gameDataPersistenceManager = (JSONPerpetualDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            gameDataPersistenceManager.DeleteGameData();
        }
    }
}

