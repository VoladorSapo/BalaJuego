using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunEnemyController))]
public class EEnemyBehaviour : Editor
{

    SerializedProperty anim;
    SerializedProperty differentFirstShootCadence;
    SerializedProperty firstShootCadence;

    SerializedProperty shootCadence;
    SerializedProperty shootCadenceRandomRange;

    SerializedProperty timeMagnitude;

    SerializedProperty stunedCollider;

    SerializedProperty area;

    SerializedProperty canBeKilledMelee;
    SerializedProperty FMarker;
    SerializedProperty ChangeMeleeCollider;

    private void OnEnable()
    {
        anim = serializedObject.FindProperty("<anim>k__BackingField");
        differentFirstShootCadence = serializedObject.FindProperty("differentFirstShootCadence");
        firstShootCadence = serializedObject.FindProperty("<firstShootCadence>k__BackingField");
        shootCadence = serializedObject.FindProperty("<shootCadence>k__BackingField");
            shootCadenceRandomRange = serializedObject.FindProperty("<shootCadenceRandomRange>k__BackingField");
        timeMagnitude = serializedObject.FindProperty("<timeMagnitude>k__BackingField");
        stunedCollider = serializedObject.FindProperty("<stunedCollider>k__BackingField");
        area = serializedObject.FindProperty("<area>k__BackingField");
        canBeKilledMelee = serializedObject.FindProperty("canBeKilledMelee");
        FMarker = serializedObject.FindProperty("FMarker");
        ChangeMeleeCollider = serializedObject.FindProperty("ChangeMeleeCollider");
    }
    public override void OnInspectorGUI()
    {
        GunEnemyController gunEnemyController = (GunEnemyController)target;

        EditorGUILayout.PropertyField(anim);
        GUILayout.Label("Shoot Cadence Configuration", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(shootCadence);
        EditorGUILayout.PropertyField(shootCadenceRandomRange);
        EditorGUILayout.PropertyField(differentFirstShootCadence);
        if (gunEnemyController.differentFirstShootCadence)
        {
            EditorGUILayout.PropertyField(firstShootCadence);
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(timeMagnitude);
        EditorGUILayout.PropertyField(stunedCollider); 
        EditorGUILayout.PropertyField(area);
        EditorGUILayout.PropertyField(canBeKilledMelee);
        EditorGUILayout.PropertyField(FMarker);
        EditorGUILayout.PropertyField(ChangeMeleeCollider);
        serializedObject.ApplyModifiedProperties();

    }
}
