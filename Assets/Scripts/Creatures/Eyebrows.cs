using UnityEngine;
using System.Collections;

public class Eyebrows : MonoBehaviour {

    public GameObject leftBrowL, leftBrowR;
    public GameObject rightBrowL, rightBrowR;

    // Use this for initialization
    void Start () {
        Sad();
        //Angry();
        //Happy();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Sad()
    {
        // left brow
        iTween.RotateTo(leftBrowL, iTween.Hash("z", -40, "time", 1));
        iTween.RotateTo(leftBrowR, iTween.Hash("z", -40, "time", 1));

        // right brow
        iTween.RotateTo(rightBrowL, iTween.Hash("z", 40, "time", 1));
        iTween.RotateTo(rightBrowR, iTween.Hash("z", 40, "time", 1));
    }

    public void Angry()
    {
        // left brow
        iTween.RotateTo(leftBrowL, iTween.Hash("z", 40, "time", 1));
        iTween.RotateTo(leftBrowR, iTween.Hash("z", 40, "time", 1));

        // right brow
        iTween.RotateTo(rightBrowL, iTween.Hash("z", -40, "time", 1));
        iTween.RotateTo(rightBrowR, iTween.Hash("z", -40, "time", 1));
    }

    public void Happy()
    {
        // left brow
        iTween.RotateTo(leftBrowL, iTween.Hash("z", -40, "time", 1));
        iTween.RotateTo(leftBrowR, iTween.Hash("z", 40, "time", 1));

        // right brow
        iTween.RotateTo(rightBrowL, iTween.Hash("z", -40, "time", 1));
        iTween.RotateTo(rightBrowR, iTween.Hash("z", 40, "time", 1));
    }

}
