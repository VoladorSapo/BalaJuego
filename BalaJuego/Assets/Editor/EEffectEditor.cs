using Codice.CM.Client.Differences.Graphic;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(EffectEditor))]

public class EEffectEditor : PropertyDrawer
{
    float y;
    float lineHeight = EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        float y = position.y;
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect NextRect(Rect position) => new Rect(position.x, y += lineHeight+ EditorGUIUtility.standardVerticalSpacing, position.width, lineHeight);

        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);
        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            SerializedProperty typeProperty = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(NextRect(position), typeProperty);
            EffectTypes effectType = (EffectTypes)typeProperty.enumValueIndex;

            SerializedProperty statProperty = property.FindPropertyRelative("stat");
            SerializedProperty durantionProperty = property.FindPropertyRelative("duration");
            SerializedProperty infiniteProperty = property.FindPropertyRelative("infinite");

            switch (effectType)
            {
                case EffectTypes.Damage:
                    EditorGUI.PropertyField(NextRect(position), statProperty, new GUIContent("Daño"));
                    break;
                case EffectTypes.AddShield:
                    EditorGUI.PropertyField(NextRect(position), statProperty, new GUIContent("Escudo"));
                    break;
                case EffectTypes.Stun:
                    EditorGUI.PropertyField(NextRect(position), infiniteProperty, new GUIContent("Infinite"));
                    EditorGUI.PropertyField(NextRect(position), durantionProperty, new GUIContent("StunDuration"));

                    break;
            }
            EditorGUI.indentLevel--;


        }
        EditorGUI.EndProperty();
    }
    private int GetLineCount(SerializedProperty property)
    {
        if (!property.isExpanded) return 1;

        SerializedProperty typeProperty = property.FindPropertyRelative("type");
        EffectTypes effectType = (EffectTypes)typeProperty.enumValueIndex;

        int lines = 2; // foldout + type

        switch (effectType)
        {
            case EffectTypes.Damage:
            case EffectTypes.AddShield:
                lines += 1;
                break;
            case EffectTypes.Stun:
                lines += 2;
                break;
        }

        return lines;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int linecount = GetLineCount(property);
        return (EditorGUIUtility.singleLineHeight * linecount) +(EditorGUIUtility.standardVerticalSpacing * Mathf.Max(0, linecount - 1)); ;
    }

}
