using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var previousGUIState = GUI.enabled;
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label);
		GUI.enabled = previousGUIState;
	}
}

[CustomPropertyDrawer(typeof(string))]
public class StringDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var field = property.serializedObject.targetObject.GetType().GetField(property.name);
		if (field != null && Attribute.GetCustomAttribute(field, typeof(ReadOnlyAttribute)) != null)
		{
			EditorGUI.PropertyField(position, property, label);
			return;
		}

		EditorGUI.BeginProperty(position, label, property);

		Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);

		Rect textAreaRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, GetPropertyHeight(property, label) - EditorGUIUtility.singleLineHeight - 4);
        GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
        textAreaStyle.wordWrap = true;

		property.stringValue = EditorGUI.TextArea(textAreaRect, property.stringValue, textAreaStyle);

        EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
		var field = property.serializedObject.targetObject.GetType().GetField(property.name);
		if (field != null && Attribute.GetCustomAttribute(field, typeof(ReadOnlyAttribute)) != null)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

       	GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
        textAreaStyle.wordWrap = true;

        float labelHeight = EditorGUIUtility.singleLineHeight;

        float textAreaHeight = textAreaStyle.CalcHeight(new GUIContent(property.stringValue), EditorGUIUtility.currentViewWidth);

        return labelHeight + textAreaHeight + 6;
    }
}