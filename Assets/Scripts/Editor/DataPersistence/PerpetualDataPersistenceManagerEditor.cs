using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerpetualJSONDataManager))]
public class PerpetualDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PerpetualJSONDataManager gameDataPersistenceManager = (PerpetualJSONDataManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            gameDataPersistenceManager.DeleteGameData();
        }
    }
}

