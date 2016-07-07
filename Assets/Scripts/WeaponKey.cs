using UnityEngine;
using System.Collections;

public class WeaponKey : MonoBehaviour {

    public enum KType { Red, Yellow, Green}

    public KType keyType = KType.Red;

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
