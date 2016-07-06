using UnityEngine;
using System.Collections;

public class Elephant : WalksOnNodes
{

    public enum CreatureState { CanInteract, Interacting, Blocked }

    public GameObject puzzlePrefab;

    public Transform puzzleHinge;
    public Transform[] puzzleCamTarget;
    public Transform puzzleCamLookTarget;

    public Animator animator;

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

        HeadPuzzle puzzle = go.GetComponent<HeadPuzzle>();
        puzzle.creature = this;

        go.transform.SetParent(puzzleHinge, false);
    }

    public void OpenHead()
    {
        animator.Play("Open");
        AudioPlayer.Instance.Play2DAudio(settings.openHeadAudio);
    }

    public void CloseHead()
    {
        animator.Play("Close");
        AudioPlayer.Instance.Play2DAudio(settings.closeHeadAudio);
    }

    // Update is called once per frame
    void Update()
    {
        if (iTween.Count(gameObject) == 0)
        {
            MoveOnNodes();
        }

        if (Input.GetKey(KeyCode.M))
        {
            if(animator != null)
            {
                OpenHead();                
            }
        }
        if (Input.GetKey(KeyCode.N))
        {
            if (animator != null)
            {
                CloseHead();
            }
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
        iTween.MoveTo(gameObject, iTween.Hash("delay", rotateTime, "position", targetNode.transform.position, "speed", settings.creatureMoveSpeed, "easetype", "linear"));
        animator.Play("Walk");
        StartCoroutine(MoveAndAnimate(rotateTime));
        //iTween.LookTo(gameObject, iTween.Hash("looktarget", targetNode.transform.position, "time", rotateTime));
        //iTween.MoveTo(gameObject, iTween.Hash("position", targetNode.transform.position, "speed", settings.creatureMoveSpeed, "easetype", "linear"));
    }

    IEnumerator MoveAndAnimate(float delay)
    {
        yield return new WaitForSeconds(delay);
        
    }

    void OnMouseUpAsButton()
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
