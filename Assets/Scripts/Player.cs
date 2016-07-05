using UnityEngine;
using System.Collections;
using System;

public class Player : WalksOnNodes {

    public enum MoveInputDirection { north, south, east, west, turnLeft, turnRight, none };

    

    private const int FORWARD = 0;
    private const int BACK = 1;
    private const int LEFT = 2;
    private const int RIGHT = 3;


    //private int nextMove = -1;
    //private MoveInputDirection nextMove = MoveInputDirection.none;

    void Start () {
	
	}
	
	void Update ()
    {
        HandleInputDown();

        if (iTween.Count(gameObject) == 0)
        {
            HandleInputHeldDown();

            MoveOnNodes();
        }

    }



    private Node GetNodeForMove(int relativeMoveDir) 
    {

        switch (GetLookDirection())
        {
            case LookDirection.north:
                switch (relativeMoveDir)
                {
                    case FORWARD:
                        return currentNode.north;
                        
                    case BACK:
                        return currentNode.south;

                    case LEFT:
                        return currentNode.west;

                    case RIGHT:
                        return currentNode.east;

                }
                break;
            case LookDirection.south:
                switch (relativeMoveDir)
                {
                    case FORWARD:
                        return currentNode.south;

                    case BACK:
                        return currentNode.north;

                    case LEFT:
                        return currentNode.east;

                    case RIGHT:
                        return currentNode.west;

                }
                break;
            case LookDirection.east:
                switch (relativeMoveDir)
                {
                    case FORWARD:
                        return currentNode.east;

                    case BACK:
                        return currentNode.west;

                    case LEFT:
                        return currentNode.north;

                    case RIGHT:
                        return currentNode.south;

                }
                break;
            case LookDirection.west:
                switch (relativeMoveDir)
                {
                    case FORWARD:
                        return currentNode.west;

                    case BACK:
                        return currentNode.east;

                    case LEFT:
                        return currentNode.south;

                    case RIGHT:
                        return currentNode.north;

                }
                break;
        }

        return null;
    }

    private LookDirection GetLookDirection()
    {
        if (Mathf.RoundToInt(transform.forward.z) == 1) return LookDirection.north;
        if (Mathf.RoundToInt(transform.forward.z) == -1) return LookDirection.south;
        if (Mathf.RoundToInt(transform.forward.x) == 1) return LookDirection.east;
        if (Mathf.RoundToInt(transform.forward.x) == -1) return LookDirection.west;

        return LookDirection.err;
    }
   

    private void HandleInputDown()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetTargetNode(GetNodeForMove(FORWARD));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetTargetNode(GetNodeForMove(BACK));
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetTargetNode(GetNodeForMove(LEFT));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetTargetNode(GetNodeForMove(RIGHT));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            turnLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            turnRight = true;
        }
    }

    

    private void HandleInputHeldDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            SetTargetNode(GetNodeForMove(FORWARD));
        }
        if (Input.GetKey(KeyCode.S))
        {
            SetTargetNode(GetNodeForMove(BACK));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            SetTargetNode(GetNodeForMove(LEFT));
        }
        if (Input.GetKey(KeyCode.E))
        {
            SetTargetNode(GetNodeForMove(RIGHT));
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnLeft = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            turnRight = true;
        }
    }

    protected override void Move()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", targetNode.transform.position, "speed", settings.playerMoveSpeed, "easetype", "linear"));
    }
}
