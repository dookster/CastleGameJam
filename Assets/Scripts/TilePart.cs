using UnityEngine;
using System.Collections;

public class TilePart : MonoBehaviour {

    public GameObject[] parts;


	// Use this for initialization
	void Start ()
    {
        GameObject thisPart = Instantiate(parts[Random.Range(0, parts.Length)]);
        //thisPart.transform.position = transform.position;
        thisPart.transform.SetParent(transform, false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
