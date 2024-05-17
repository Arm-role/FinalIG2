using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIManager))]
public class UIModified : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UIManager changeColor = (UIManager)target;

        if (GUILayout.Button("Change Color"))
        {
            changeColor.ChangeColorImage();
        }
    }
}
