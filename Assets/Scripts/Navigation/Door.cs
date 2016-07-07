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
        AudioPlayer.Instance.Play2DAudio(Player.Instance.settings.openDoorAudio);
    }

    IEnumerator OpenAnimation()
    {

        foreach (GameObject go in pieces)
        {
            Rigidbody body = go.GetComponent<Rigidbody>();
            Collider col = go.GetComponent<Collider>();
            col.enabled = true;
            body.useGravity = true;
            body.AddExplosionForce(200, transform.position - transform.forward, 2);
        }
        yield return new WaitForSeconds(1f);
        currentNode.locked = false;
        currentNode.door = null;
        foreach (GameObject go in pieces)
        {
            Rigidbody body = go.GetComponent<Rigidbody>();
            Collider coll = go.GetComponent<Collider>();

            body.isKinematic = true;
            coll.enabled = false;
        }
        Player.Instance.RemoveItem();
    }


}
