using UnityEngine;
using System.Collections;
using System;

public class Elephant : WalksOnNodes
{

    public enum CreatureState { CanInteract, Interacting, Happy, Jumping }

    public GameObject puzzlePrefab;

    public Transform puzzleHinge;
    public Transform[] puzzleCamTarget;
    public Transform puzzleCamLookTarget;

    public Animator animator;

    public BoxCollider boxCollider;

    public Eyebrows eyebrows;

    public ParticleSystem dieParticles;

    public AudioSource stepAudioSource;
    public AudioSource turnAudioSource;

    public bool fadeToPink = false;
    private float fadeTime = 0.5f;
    

    public bool moving = true;

    public CreatureState currentState = CreatureState.CanInteract;

    public float moveInterval = 2f;
    private float lastStep = 0;

    public Renderer rend;

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

        if (fadeToPink)
        {
            rend.material.SetFloat("_Blend", rend.material.GetFloat("_Blend") + fadeTime * Time.deltaTime);
        }
    }
    

    protected override void Move()
    {
        float rotateTime = 0.2f;

        if(Vector3.Distance(targetNode.transform.position, transform.position + transform.forward) > 0.1f)
        {
            turnAudioSource.Play();
        }

        iTween.LookTo(gameObject, iTween.Hash("looktarget", targetNode.transform.position, "time", rotateTime));
        iTween.MoveTo(gameObject, iTween.Hash("delay", rotateTime, "position", targetNode.transform.position, "speed", settings.creatureMoveSpeed, "easetype", "linear"));
        animator.Play("Walk");
        stepAudioSource.Play();
        //StartCoroutine(MoveAndAnimate(rotateTime));
        //iTween.LookTo(gameObject, iTween.Hash("looktarget", targetNode.transform.position, "time", rotateTime));
        //iTween.MoveTo(gameObject, iTween.Hash("position", targetNode.transform.position, "speed", settings.creatureMoveSpeed, "easetype", "linear"));
    }

    //IEnumerator MoveAndAnimate(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //}

    

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

        if(currentState == CreatureState.Happy &&
            iTween.Count(Player.Instance.gameObject) == 0 &&
            Player.Instance.interactingCreature == null &&
            currentNode.IsNeighbour(Player.Instance.currentNode))
        {
            currentState = CreatureState.Jumping;
            JumpToNode(Player.Instance.currentNode);
            Player.Instance.JumpToNode(currentNode);

            moving = false;

            // switch nodes
            Node tempNode = currentNode;
            currentNode = Player.Instance.currentNode;
            Player.Instance.currentNode = tempNode;
        }
    }

    public override void JumpToNode(Node jumpNode)
    {
        StartCoroutine(Jump(jumpNode));

    }

    IEnumerator Jump(Node jumpNode)
    {
        float jumpSpeed = 3f;
        float rotateTime = 0.2f;
        iTween.LookTo(gameObject, iTween.Hash("looktarget", jumpNode.transform.position, "time", rotateTime));
        if (Vector3.Distance(jumpNode.transform.position, transform.forward) > 0.1f)
        {
            turnAudioSource.Play();
        }
        yield return new WaitForSeconds(rotateTime);
        Vector3[] jumpPath = new Vector3[2];
        jumpPath[0] = transform.position + (transform.forward + transform.right) / 4;
        jumpPath[1] = jumpNode.transform.position;
        iTween.MoveTo(gameObject, iTween.Hash("path", jumpPath, "speed", jumpSpeed, "easetype", "linear"));
        animator.Play("Walk");
        stepAudioSource.Play();
        yield return new WaitForSeconds(1f);
        currentState = CreatureState.Happy;
        moving = true;
    }

}
