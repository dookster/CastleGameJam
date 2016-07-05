using UnityEngine;
using System.Collections;

public abstract class WalksOnNodes : MonoBehaviour {

    public enum LookDirection { north, south, east, west, err }

    public Settings settings;

    public Node currentNode;
    protected Node targetNode;

    protected bool turnLeft = false;
    protected bool turnRight = false;

    // Use this for initialization
    void Start () {
        currentNode.locked = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void MoveOnNodes()
    {
        if (targetNode != null)
        {
            Move();
            currentNode.locked = false;
            currentNode = targetNode;
            targetNode = null;
        }
        else if (turnLeft)
        {
            iTween.RotateBy(gameObject, iTween.Hash("y", -(1f / 4f), "time", settings.playerTurnSpeed, "easetype", "linear"));
            turnLeft = false;
        }
        else if (turnRight)
        {
            iTween.RotateBy(gameObject, iTween.Hash("y", (1f / 4f), "time", settings.playerTurnSpeed, "easetype", "linear"));
            turnRight = false;
        }
    }

    protected abstract void Move();

    protected void SetTargetNode(Node target)
    {
        targetNode = target;
        if(targetNode != null) targetNode.locked = true;
    }
}
