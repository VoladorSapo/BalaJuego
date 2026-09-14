using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyLife))]
public class EEnemyLife : Editor
{
    int stunTime = 1;
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
            stunTime = EditorGUILayout.IntField("Stun Time: (<0 = infinito)",stunTime);
            if (GUILayout.Button("Stun"))
            {
                Debug.Log("STUN FOR SECONDS: "+ stunTime);
                
                    data.addEffect(new StunEffect(stunTime<0, stunTime));
                
            }
        }
    }
}
