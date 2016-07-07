using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public TilePart northWall;
    public TilePart southWall;
    public TilePart eastWall;
    public TilePart westWall;
    public TilePart floor;
    public TilePart ceiling;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetNode(Node node)
    {

        if (northWall != null) northWall.gameObject.SetActive(node.north == null);
        if (southWall != null) southWall.gameObject.SetActive(node.south == null);
        if (eastWall != null) eastWall.gameObject.SetActive(node.east == null);
        if (westWall != null) westWall.gameObject.SetActive(node.west == null);
    }
}
