using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

    public Node north;
    public Node south;
    public Node east;
    public Node west;

    public GameObject tilePrefab;

    private Vector3 gizmoSize = new Vector3(0.5f, 0.1f, 0.5f);

    [Space()]
    public Settings settings;

    // Use this for initialization
    void Start ()
    {
        // Spawn tile graphics
        GameObject go= Instantiate(tilePrefab, transform.position, transform.rotation) as GameObject;
        go.GetComponent<Tile>().SetNode(this);
	}
	
	// Update is called once per frame
	void Update () {
	
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
