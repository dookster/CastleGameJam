using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor {

    public float nodeDistance = 1;

    private int graphicIndex = 0;

    private Node node;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        node = (Node)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add node");

        if (GUILayout.Button("North"))
        {
            if(node.north == null)
            {
                node.north = AddNode(node.transform.position + new Vector3(0, 0, nodeDistance));
                node.north.south = node;
            }
        }

        if (GUILayout.Button("South"))
        {
            if (node.south == null)
            {
                node.south = AddNode(node.transform.position + new Vector3(0, 0, -nodeDistance));
                node.south.north = node;
            }
        }

        if (GUILayout.Button("East"))
        {
            if (node.east == null)
            {
                node.east = AddNode(node.transform.position + new Vector3(nodeDistance, 0, 0));
                node.east.west = node;
            }
        }

        if (GUILayout.Button("West"))
        {
            if (node.west == null)
            {
                node.west = AddNode(node.transform.position + new Vector3(-nodeDistance, 0, 0));
                node.west.east = node;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Graphics");
        graphicIndex = EditorGUILayout.Popup(graphicIndex, graphicNames().ToArray());
        node.tilePrefab = node.settings.tilePrefabs[graphicIndex];
    }

    Node AddNode(Vector3 position)
    {
        Node newNode = Instantiate(node) as Node;
        newNode.Clear();
        newNode.transform.position = position;
        newNode.transform.SetParent(node.transform.parent, true);
        newNode.name = "Node";
        Undo.RegisterCreatedObjectUndo(newNode.gameObject, "Created node");
        Selection.activeGameObject = newNode.gameObject;
        return newNode;
    }

    List<string> graphicNames()
    {
        List<string> result = new List<string>();
        foreach(GameObject go in node.settings.tilePrefabs)
        {
            result.Add(go.name);
        }
        return result;
    }


}
