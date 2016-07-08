using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Debris : MonoBehaviour {

    public float rotateSpeed;

	// Use this for initialization
	void Start ()
    {
        for(int n = 0; n < transform.childCount; n++)
        {
            transform.GetChild(n).localEulerAngles = new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
        }


        iTween.RotateBy(gameObject, iTween.Hash("x", 10, "speed", rotateSpeed, "easetype", "linear", "looptype", "loop"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
