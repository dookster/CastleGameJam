using UnityEngine;
using System.Collections;

public class ExitCrystal : MonoBehaviour {

    public float rotateSpeed;

	// Use this for initialization
	void Start ()
    {
        iTween.RotateBy(gameObject, iTween.Hash("y", 10, "speed", rotateSpeed, "easetype", "linear", "looptype", "loop"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
