using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Grid pathManager = (Grid)target;

        //EditorGUILayout.LabelField("Length", pathManager.Length() + "");
    }

}
