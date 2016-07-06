using UnityEngine;
using System.Collections;

public class WeaponKey : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUpAsButton()
    {
        if(iTween.Count(gameObject) == 0)
        {
            Player.Instance.SwingItem();
        }
    }

}
