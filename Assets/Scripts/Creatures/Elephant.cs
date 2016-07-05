using UnityEngine;
using System.Collections;

public class Elephant : WalksOnNodes {

    public GameObject puzzlePrefab;

    public Transform puzzleHinge;

    public float moveInterval = 2f;
    private float lastStep = 0;

	// Use this for initialization
	void Start () {
        GameObject go = Instantiate(puzzlePrefab);

        go.transform.SetParent(puzzleHinge, false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (iTween.Count(gameObject) == 0)
        {
            MoveOnNodes();
        }


        // move around!
        if(Time.time - lastStep > moveInterval)
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

}
