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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(interactingCreature != null)
            {
                StopInteractingWithCreature();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TryToOpenDoor(WeaponKey.KType.Green);
            TryToOpenDoor(WeaponKey.KType.Red);
            TryToOpenDoor(WeaponKey.KType.Yellow);
        }

        // Swing weaponkey
        if (interactingCreature == null && Input.GetMouseButtonDown(1) && iTween.Count(gameObject) == 0)
        {
            SwingItem();
        }

        if (interactingCreature == null && canMove) HandleInputDown();

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
        Debug.Log("Picking up item");
        // move to holder
        canMove = false;
        AudioPlayer.Instance.Play2DAudio(settings.pickUpAudio);
        StartCoroutine(MoveRotatePickup(itemGraphic));
    }

    IEnumerator MoveRotatePickup(GameObject itemGraphic)
    {
        float moveTime = 1f;
        //float rotateTime = 0.5f;
        //iTween.RotateTo(itemGraphic, iTween.Hash("rotation", itemHolder.transform, "time", rotateTime));
        //yield return new WaitForSeconds(rotateTime);
        iTween.MoveTo(itemGraphic, iTween.Hash("position", itemHolder.position, "time", moveTime, "looktarget", (itemHolder.position - itemHolder.forward)));
        yield return new WaitForSeconds(moveTime);
        itemGraphic.transform.SetParent(itemHolder);
        itemGraphic.layer = LayerMask.NameToLayer("weapon");
        canMove = true;
    }

    public void RemoveItem()
    {
        if (itemHolder.childCount == 0) return;
        GameObject item = itemHolder.GetChild(0).gameObject;
        if (item != null)
        {
            //StartCoroutine(MoveRemovedItem(item));
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().isKinematic = false;
            item.GetComponent<Rigidbody>().AddForce(transform.forward * 50);
            item.layer = LayerMask.NameToLayer("Default");
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

    public void SwingItem()
    {
        if (iTween.Count(itemHolder.gameObject) == 0 && itemHolder.childCount > 0)
        {
            StartCoroutine(RotateSwingingItem());
            AudioPlayer.Instance.Play2DAudio(settings.swingWeaponAudio, 0.5f);
        }
    }
    
    IEnumerator RotateSwingingItem()
    {
        float rotateTime = 1f;
        iTween.PunchRotation(itemHolder.gameObject, iTween.Hash("x", 90f, "time", rotateTime));
        yield return new WaitForSeconds(rotateTime/10);
        WeaponKey key = itemHolder.GetComponentInChildren<WeaponKey>();
        TryToOpenDoor(key.keyType);
    }

    private void TryToOpenDoor(WeaponKey.KType keyType)
    {
        // Get any door we're at
        Node facingNode = GetNodeForMove(FORWARD);
        if (facingNode != null && facingNode.door != null)
        {
            if (facingNode.door.keyType == keyType)
            {
                facingNode.door.Open();
            }
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
        AudioClip stepclip = settings.footstepAudio[UnityEngine.Random.Range(0, settings.footstepAudio.Length)];
        AudioPlayer.Instance.Play2DAudio(stepclip);

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
        AudioPlayer.Instance.Play2DAudio(settings.creatureAngryAudio);
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
        yield return new WaitForSeconds(delay * 2);
        interactingCreature.fadeToPink = true;
        yield return new WaitForSeconds(delay);
        AudioPlayer.Instance.Play2DAudio(settings.creatureHappyAudio);

        interactingCreature.eyebrows.Happy();

        yield return new WaitForSeconds(delay);
        interactingCreature.currentState = Elephant.CreatureState.Happy;
        interactingCreature.boxCollider.enabled = true;
        interactingCreature.moving = true;
        interactingCreature = null;


        //if (interactingCreature.dieParticles != null) interactingCreature.dieParticles.gameObject.SetActive(true);

        //yield return new WaitForSeconds(delay);
        //iTween.MoveBy(interactingCreature.gameObject, iTween.Hash("y", -2, "time", 5));
        //interactingCreature.currentNode.locked = false;
    }

    private void ZoomToPuzzle(Transform[] camTarget, Transform puzzle)
    {
        AudioPlayer.Instance.Play2DAudio(settings.weirdHum);
        StartCoroutine(ZoomAndOpen(camTarget, puzzle));
    }

    IEnumerator ZoomAndOpen(Transform[] camTarget, Transform puzzle)
    {
        iTween.MoveTo(mainCam.gameObject, iTween.Hash("path", camTarget, "looktarget", puzzle, "time", settings.cameraZoomSpeed, "easetype", "linear"));
        yield return new WaitForSeconds(0.25f);
        interactingCreature.OpenHead();
    }

    private void ReturnCamera()
    {
        interactingCreature.CloseHead();
        //iTween.MoveTo(mainCam.gameObject, iTween.Hash("position", cameraOrigin, "time", settings.cameraZoomSpeed, "easetype", "linear"));
        List<Transform> returnPath = new List<Transform>();        
        returnPath.AddRange(interactingCreature.puzzleCamTarget);
        returnPath.Reverse();
        returnPath.Add(cameraOrigin);
        AudioPlayer.Instance.Play2DAudio(settings.weirdHum);
        iTween.MoveTo(mainCam.gameObject, iTween.Hash("path", returnPath.ToArray(), "time", settings.cameraZoomSpeed, "easetype", "linear"));
        iTween.RotateTo(mainCam.gameObject, iTween.Hash("rotation", cameraOrigin, "time", settings.cameraZoomSpeed, "easetype", "linear"));
    }

    IEnumerator CloseAndReturn()
    {
        yield return new WaitForSeconds(1f);
    }

    public  override void JumpToNode(Node jumpNode)
    {
        StartCoroutine(Jump(jumpNode));
        canMove = false;
    }

    IEnumerator Jump(Node jumpNode)
    {
        float jumpSpeed = 3f;
        float rotateTime = 0.2f;
        yield return new WaitForSeconds(rotateTime);
        
        Vector3[] jumpPath = new Vector3[2];
        jumpPath[0] = transform.position + (transform.forward + transform.right) / 4;
        jumpPath[1] = jumpNode.transform.position;
        iTween.MoveTo(gameObject, iTween.Hash("path", jumpPath, "speed", jumpSpeed, "easetype", "linear"));
        yield return new WaitForSeconds(1f);
        canMove = true;
    }
}
