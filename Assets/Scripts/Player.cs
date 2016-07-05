using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : WalksOnNodes {

    public enum MoveInputDirection { north, south, east, west, turnLeft, turnRight, none };

    public Camera mainCam;
    public Transform cameraOrigin;

    public Elephant interactingCreature = null;

    private const int FORWARD = 0;
    private const int BACK = 1;
    private const int LEFT = 2;
    private const int RIGHT = 3;

    private static Player instance = null;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(Player)) as Player;
            }
            return instance;
        }
    }

    //private int nextMove = -1;
    //private MoveInputDirection nextMove = MoveInputDirection.none;

    protected override void Start () {
        base.Start();
	}
	
	void Update ()
    {
        if(interactingCreature == null) HandleInputDown();

        if (iTween.Count(gameObject) == 0)
        {
            if (interactingCreature == null) HandleInputHeldDown();

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

    public void InteractWithCreature(Elephant creature)
    {
        StartCoroutine(TurnAndZoom(creature));
    }

    // turn creature and zoom to puzzle
    IEnumerator TurnAndZoom(Elephant creature)
    {
        iTween.LookTo(creature.gameObject, iTween.Hash("looktarget", transform, "time", 0.2f));
        yield return new WaitForSeconds(0.2f);
        ZoomToPuzzle(creature.puzzleCamTarget, creature.puzzleCamLookTarget);
        interactingCreature = creature;

    }

    public void StopInteractingWithCreature()
    {
        ReturnCamera();

        // TEMP
        iTween.MoveBy(interactingCreature.gameObject, iTween.Hash("y", -2, "time", 2, "delay", settings.cameraZoomSpeed));
        interactingCreature.currentNode.locked = false;
        interactingCreature = null;
    }

    private void ZoomToPuzzle(Transform camTarget, Transform puzzle)
    {
        iTween.MoveTo(mainCam.gameObject, iTween.Hash("position", camTarget, "looktarget", puzzle, "time", settings.cameraZoomSpeed, "easetype", "linear"));
    }

    private void ReturnCamera()
    {
        iTween.MoveTo(mainCam.gameObject, iTween.Hash("position", cameraOrigin, "time", settings.cameraZoomSpeed, "easetype", "linear"));
        iTween.RotateTo(mainCam.gameObject, iTween.Hash("rotation", cameraOrigin, "time", settings.cameraZoomSpeed, "easetype", "linear"));
    }
}
