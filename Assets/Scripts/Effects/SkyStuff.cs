using UnityEngine;
using System.Collections;

public class SkyStuff : MonoBehaviour {

    public Skybox skybox;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RenderSettings.skybox.SetFloat("_Exposure", 5);
    }
}
