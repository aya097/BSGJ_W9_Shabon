using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(BubbleDataBase))]

/// <summary>
/// BubbleDataBaseのエディタ拡張クラス
/// </summary>
/// 
/// <remarks>
/// BubbleDataBaseのListのインスペクタの要素名をBubbleTypeのenum名に変更する
/// </remarks>
public class BubbleDataBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var bubbleDataProp = serializedObject.FindProperty("bubbleData");

        // サイズ変更UI
        bubbleDataProp.arraySize = EditorGUILayout.IntField("Size", bubbleDataProp.arraySize);

        EditorGUI.indentLevel++;
        for (int i = 0; i < bubbleDataProp.arraySize; i++)
        {
            var element = bubbleDataProp.GetArrayElementAtIndex(i);
            var bubbleTypeProp = element.FindPropertyRelative("bubbleType");
            string[] enumNames = bubbleTypeProp.enumDisplayNames;
            string label = (i < enumNames.Length) ? enumNames[i] : $"Element {i}";

            EditorGUILayout.PropertyField(element, new GUIContent(label), true);
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif