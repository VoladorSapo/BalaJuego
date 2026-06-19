using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyLife))]
public class EEnemyLife : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EnemyLife data = (EnemyLife)target;
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Die"))
            {
                data.Die();
            }
        }
    }
}
