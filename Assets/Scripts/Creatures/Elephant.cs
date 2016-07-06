using UnityEngine;
using System.Collections;

public class Elephant : WalksOnNodes
{

    public enum CreatureState { CanInteract, Interacting, Blocked }

    public GameObject puzzlePrefab;

    public Transform puzzleHinge;
    public Transform puzzleCamTarget;
    public Transform puzzleCamLookTarget;

    public BoxCollider boxCollider;

    public Eyebrows eyebrows;

    public bool moving = true;

    public CreatureState currentState = CreatureState.CanInteract;

    public float moveInterval = 2f;
    private float lastStep = 0;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        GameObject go = Instantiate(puzzlePrefab);

        go.transform.SetParent(puzzleHinge, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (iTween.Count(gameObject) == 0)
        {
            MoveOnNodes();
        }


        // move around!
        if (moving && Time.time - lastStep > moveInterval)
        {
            Node target = currentNode.GetRandomNeighbour();
            if (target != null) SetTargetNode(target);

            lastStep = Time.time;
        }
    }

    protected override void Move()
    {
        float rotateTime = 0.2f;

        iTween.LookTo(gameObject, iTween.Hash("looktarget", targetNode.transform.position, "time", rotateTime));
        iTween.MoveTo(gameObject, iTween.Hash("delay", rotateTime, "position", targetNode.transform.position, "speed", settings.playerMoveSpeed, "easetype", "linear"));
    }

    void OnMouseDown()
    {
        if(currentState == CreatureState.CanInteract && 
            iTween.Count(Player.Instance.gameObject) == 0 && 
            Player.Instance.interactingCreature == null &&
            currentNode.IsNeighbour(Player.Instance.currentNode))
        {
            
            currentState = CreatureState.Interacting;
            Player.Instance.InteractWithCreature(this);
            boxCollider.enabled = false;
            moving = false;
        }
    }

}
