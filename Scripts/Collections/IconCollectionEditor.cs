using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(IconCollection))]
public class IconCollectionEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var collection = (IconCollection)target;

        if (GUILayout.Button("Refresh"))
        {
            collection.Refresh();
        }
    }
}
#endif