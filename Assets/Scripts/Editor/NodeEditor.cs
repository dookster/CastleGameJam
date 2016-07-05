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

        if (GUILayout.Button("Guess neighbours"))
        {
            GuessNeighbours();
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

    private void GuessNeighbours()
    {
        Debug.Log("GUESSING");

        Debug.Log("" + (node.transform.position + new Vector3(0, 0, 1)));

        if(node.north == null)
        {
            node.north = FindNodeAt(node.transform.position + new Vector3(0, 0, 1));
            if(node.north != null) node.north.south = node;
        }
        if (node.south == null)
        {
            node.south = FindNodeAt(node.transform.position + new Vector3(0, 0, -1));
            if (node.south!= null) node.south.north = node;
        }
        if (node.east == null)
        {
            node.east = FindNodeAt(node.transform.position + new Vector3(1, 0, 0));
            if (node.east != null) node.east.west = node;
        }
        if (node.west == null)
        {
            node.west = FindNodeAt(node.transform.position + new Vector3(-1, 0, 0));
            if (node.west != null) node.west.east = node;
        }

    }

    private Node FindNodeAt(Vector3 pos)
    {
        // bla bla
        List<Node> allNodes = new List<Node>();
        allNodes.AddRange(FindObjectsOfType<Node>());
        Debug.Log("Found nodes: " + allNodes.Count + "," + pos);

        foreach(Node n in allNodes)
        {
            if (Vector3.Distance(pos, n.transform.position) < 0.01f)
            {
                return n;
            }
        }
        return null;
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
