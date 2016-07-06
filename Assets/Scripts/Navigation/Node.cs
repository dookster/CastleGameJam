using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public Node north;
    public Node south;
    public Node east;
    public Node west;

    public bool locked = false;

    public GameObject tilePrefab;

    [HideInInspector]
    public Door door;

    private Vector3 gizmoSize = new Vector3(0.5f, 0.1f, 0.5f);

    [Space()]
    public Settings settings;

    // Use this for initialization
    void Start ()
    {
        // Spawn tile graphics
        GameObject go= Instantiate(tilePrefab, transform.position, transform.rotation) as GameObject;
        go.GetComponent<Tile>().SetNode(this);
        go.transform.SetParent(transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsNeighbour(Node otherNode)
    {
        if (north == otherNode) return true;
        if (south == otherNode) return true;
        if (east == otherNode) return true;
        if (west== otherNode) return true;

        return false;
    }

    public Node GetRandomNeighbour()
    {
        List<Node> availableNodes = new List<Node>();
        if (north != null && !north.locked)
        {
            availableNodes.Add(north);
        }
        if (south != null && !south.locked)
        {
            availableNodes.Add(south);
        }
        if (east != null && !east.locked)
        {
            availableNodes.Add(east);
        }
        if (west!= null && !west.locked)
        {
            availableNodes.Add(west);
        }

        if(availableNodes.Count > 0)
        {
            return availableNodes[Random.Range(0, availableNodes.Count)];
        }
        else
        {
            return null;
        }

        //switch (Random.Range(0, 4))
        //{
        //    case 0:
        //        return north;
        //    case 1:
        //        return south;
        //    case 2:
        //        return east;
        //    case 3:
        //        return west;
        //}

        //return null;
    }

    public void Clear()
    {
        north = null;
        south = null;
        east = null;
        west = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position - new Vector3(0, 0.5f, 0), gizmoSize);

        Gizmos.color = new Color(1, 0.5f, 0.5f);
        if (north != null)
        {
            DrawGizmoLineto(north.transform.position);
        }
        if (south != null)
        {
            DrawGizmoLineto(south.transform.position);
        }
        if (east != null)
        {
            DrawGizmoLineto(east.transform.position);
        }
        if (west != null)
        {
            DrawGizmoLineto(west.transform.position);
        }

    }

    void DrawGizmoLineto(Vector3 position)
    {
        Gizmos.DrawLine(transform.position - new Vector3(0, 0.5f, 0), position - new Vector3(0, 0.5f, 0));
    }

}
