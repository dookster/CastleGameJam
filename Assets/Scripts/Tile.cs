using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetNode(Node node)
    {
        northWall.SetActive(node.north == null);
        southWall.SetActive(node.south == null);
        eastWall.SetActive(node.east == null);
        westWall.SetActive(node.west == null);
    }
}
