using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {

    public Node currentNode;
    public GameObject itemGraphic;

	// Use this for initialization
	void Start ()
    {
        currentNode.locked = true;
        IdleAnimate();
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
        if (iTween.Count(Player.Instance.gameObject) == 0 &&
            Player.Instance.interactingCreature == null &&
            currentNode.IsNeighbour(Player.Instance.currentNode))
        {
            StopIdleAnimaion();
            Player.Instance.PickupItem(itemGraphic);
            currentNode.locked = false;
        }
    }
}
