using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

    public Node currentNode;
    public GameObject itemGraphic;

    private Collider col;

	// Use this for initialization
	void Start ()
    {
        currentNode.locked = true;
        IdleAnimate();
        col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void IdleAnimate()
    {
        itemGraphic.transform.Rotate(new Vector3(0, 0, 20f));
        iTween.RotateBy(itemGraphic, iTween.Hash("y", 5f, "speed", 50f, "looptype", "loop", "space", Space.World, "easetype", "linear"));
    }

    public void StopIdleAnimaion()
    {
        iTween.Stop(itemGraphic);
    }

    void OnMouseUpAsButton()
    {
        Debug.Log("Pick up clicked " + iTween.Count(Player.Instance.gameObject) + " - " + currentNode.IsNeighbour(Player.Instance.currentNode));
        if (iTween.Count(Player.Instance.gameObject) == 0 &&
            Player.Instance.interactingCreature == null &&
            currentNode.IsNeighbour(Player.Instance.currentNode))
        {
            col.enabled = false;
            StopIdleAnimaion();
            Player.Instance.PickupItem(itemGraphic);
            currentNode.locked = false;
        }
    }
}
