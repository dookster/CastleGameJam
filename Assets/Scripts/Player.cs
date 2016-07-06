using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : WalksOnNodes {

    public enum MoveInputDirection { north, south, east, west, turnLeft, turnRight, none };

    public Camera mainCam;
    public Transform cameraOrigin;

    public Transform itemHolder;

    public Elephant interactingCreature = null;

    private const int FORWARD = 0;
    private const int BACK = 1;
    private const int LEFT = 2;
    private const int RIGHT = 3;

    [HideInInspector]
    public bool canMove = true;

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
        // TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwingItem();
            //RemoveItem();
        }


        if(interactingCreature == null && canMove) HandleInputDown();

        if (iTween.Count(gameObject) == 0)
        {
            if (interactingCreature == null && canMove) HandleInputHeldDown();

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

    public void PickupItem(GameObject itemGraphic)
    {
        // move to holder
        canMove = false;

        StartCoroutine(MoveRotatePickup(itemGraphic));
    }

    IEnumerator MoveRotatePickup(GameObject itemGraphic)
    {
        float moveTime = 1f;
        //float rotateTime = 0.5f;
        //iTween.RotateTo(itemGraphic, iTween.Hash("rotation", itemHolder.transform, "time", rotateTime));
        //yield return new WaitForSeconds(rotateTime);
        iTween.MoveTo(itemGraphic, iTween.Hash("position", itemHolder.position, "time", moveTime, "looktarget", (itemHolder.position + itemHolder.forward)));
        yield return new WaitForSeconds(moveTime);
        itemGraphic.transform.SetParent(itemHolder);

        canMove = true;
    }

    void RemoveItem()
    {
        GameObject item = itemHolder.GetChild(0).gameObject;
        if (item != null)
        {
            StartCoroutine(MoveRemovedItem(item));
        }
    }

    IEnumerator MoveRemovedItem(GameObject item)
    {
        float moveTime = 1;
        iTween.MoveBy(item, iTween.Hash("y", -2, "time", moveTime));
        yield return new WaitForSeconds(moveTime);
        item.transform.SetParent(null);
        Destroy(item);
    }

    void SwingItem()
    {
        if (iTween.Count(itemHolder.gameObject) == 0 && itemHolder.childCount > 0)
        {
            StartCoroutine(RotateSwingingItem());
        }
    }
    
    IEnumerator RotateSwingingItem()
    {
        float rotateTime = 1f;
        iTween.PunchRotation(itemHolder.gameObject, iTween.Hash("x", 90f, "time", rotateTime));
        yield return new WaitForSeconds(rotateTime/5);

        // Get any door we're at
        Node facingNode = GetNodeForMove(FORWARD);
        if(facingNode != null && facingNode.door != null)
        {
            facingNode.door.Open();
        }
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
        interactingCreature = creature;
    }

    // turn creature and zoom to puzzle
    IEnumerator TurnAndZoom(Elephant creature)
    {
        iTween.LookTo(creature.gameObject, iTween.Hash("looktarget", transform, "time", 0.2f));
        yield return new WaitForSeconds(0.2f);
        creature.eyebrows.Angry();
        yield return new WaitForSeconds(1f);
        ZoomToPuzzle(creature.puzzleCamTarget, creature.puzzleCamLookTarget);
        
    }

    public void StopInteractingWithCreature()
    {
        ReturnCamera();
        
        StartCoroutine(MoveCreatureAndUnlock(1f));
    }

    IEnumerator MoveCreatureAndUnlock(float delay)
    {
        yield return new WaitForSeconds(delay);
        interactingCreature.eyebrows.Happy();
        yield return new WaitForSeconds(delay);
        iTween.MoveBy(interactingCreature.gameObject, iTween.Hash("y", -2, "time", 2));
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
