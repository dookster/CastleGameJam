using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public Node currentNode;

    public GameObject[] pieces;

	// Use this for initialization
	void Start () {
        currentNode.locked = true;
        currentNode.door = this;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKey(KeyCode.U))
        //{
        //    Open();
        //}
	}

    public void Open()
    {
        StartCoroutine(OpenAnimation());
    }

    IEnumerator OpenAnimation()
    {

        foreach (GameObject go in pieces)
        {
            Rigidbody body = go.GetComponent<Rigidbody>();
            body.AddExplosionForce(200, transform.position - transform.forward, 2);
        }
        yield return new WaitForSeconds(1f);
        currentNode.locked = false;
        currentNode.door = null;
        foreach (GameObject go in pieces)
        {
            Rigidbody body = go.GetComponent<Rigidbody>();
            BoxCollider coll = go.GetComponent<BoxCollider>();

            body.isKinematic = true;
            coll.enabled = false;
        }
    }


}
