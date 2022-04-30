using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AvatarSavingSystem
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PartAttachment))]
    public class PartAttachmentEditor : Editor
    {
		private bool[] OverrideFoldouts = null;

		public override void OnInspectorGUI()
		{

			serializedObject.Update();

			SerializedProperty Script = serializedObject.FindProperty("m_Script");
			SerializedProperty RetargetPosition = serializedObject.FindProperty("RetargetPosition");
			SerializedProperty RetargetRotation = serializedObject.FindProperty("RetargetRotation");
			SerializedProperty RetargetScale = serializedObject.FindProperty("RetargetScale");

			GUI.enabled = false;
			EditorGUILayout.PropertyField(Script, true, new GUILayoutOption[0]);
			GUI.enabled = true;

			EditorGUI.showMixedValue = RetargetPosition.hasMultipleDifferentValues;
			RetargetPosition.boolValue = EditorGUILayout.Toggle("Retarget Position", RetargetPosition.boolValue);
			EditorGUI.showMixedValue = RetargetRotation.hasMultipleDifferentValues;
			RetargetRotation.boolValue = EditorGUILayout.Toggle("Retarget Rotation", RetargetRotation.boolValue);
			EditorGUI.showMixedValue = RetargetScale.hasMultipleDifferentValues;
			RetargetScale.boolValue = EditorGUILayout.Toggle("Retarget Scale", RetargetScale.boolValue);

			if (!serializedObject.isEditingMultipleObjects)
			{

				EditorGUI.showMixedValue = false;
				PartAttachment element = (PartAttachment)serializedObject.targetObject;
				PartAttachmentOverride[] overrides = element.GetComponentsInChildren<PartAttachmentOverride>();
				Array.Resize(ref OverrideFoldouts, overrides.Length);
				for (int i = 0; i < overrides.Length; i++)
				{
					OverrideFoldouts[i] = EditorGUILayout.Foldout(OverrideFoldouts[i], "Retarget Override");
					if (OverrideFoldouts[i])
					{
						EditorGUI.indentLevel++;
						GUI.enabled = false;
						EditorGUILayout.ObjectField("Bone", overrides[i], typeof(PartAttachmentOverride), true);
						GUI.enabled = true;
						overrides[i].RetargetPosition = EditorGUILayout.Toggle("Retarget Position", overrides[i].RetargetPosition);
						overrides[i].RetargetRotation = EditorGUILayout.Toggle("Retarget Rotation", overrides[i].RetargetRotation);
						overrides[i].RetargetScale = EditorGUILayout.Toggle("Retarget Scale", overrides[i].RetargetScale);
						EditorGUI.indentLevel--;
					}
				}

			}

			serializedObject.ApplyModifiedProperties();

		}
	}
}