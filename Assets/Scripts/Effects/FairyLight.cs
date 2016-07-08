using UnityEngine;
using System.Collections;

public class FairyLight : MonoBehaviour {

    private Vector3 initialpos;
    private float initalMeshScale;
    private float seed;
    private Light lit;
    private float initialIntesity;

	// Use this for initialization
	void Start ()
    {
	 initialpos = gameObject.transform.position;
     initalMeshScale = gameObject.transform.localScale.x;
     seed = initialpos.x * initialpos.y * initialpos.z + initialpos.x + initialpos.y + initialpos.z;

     Light lit = gameObject.GetComponent<Light>();
    initialIntesity = lit.intensity;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Vector3 pos = gameObject.transform.position;

        //Vector3 pos = initialpos;
        Vector3 pos = new Vector3(0,0,0);
        float scale = 0.5f;
        float speed = 4f;
        float yscale = 0.5f;
        float yspeed = 0.5f;
        float xoffset = 1.2f;
        float overallScale = 0.2f;
       
        pos.x+=(Mathf.Sin((Time.time+seed)) * speed)*scale;
        // additional x modification
        pos.z+=(Mathf.Cos((Time.time+seed)) * speed)*scale;
        //pos.x += (Mathf.Sin(Time.time*0.3f)+0.6f);
        // pos.z+=Mathf.Cos(Time.time*speed)*scale;
        pos.z += Mathf.Cos((Time.time *0.2f)+ seed) * (Mathf.Sin(Time.time*1.3f)*5.3f);

        pos.y+=Mathf.Sin((Time.time * yspeed)+ seed) * yscale;

        pos *= overallScale;
        pos = initialpos + pos;

        gameObject.transform.position = pos;


        float intensity = Mathf.Sin(Time.time*1f + seed);

        //Change light depending on intensity
        lit = gameObject.GetComponent<Light>();
        //lit.intensity =  Mathf.Max(initialIntesity* 0.5f, initialIntesity *(intensity * 0.5f + 0.5f))
        lit.intensity = (initialIntesity * 0.5f) + initialIntesity * (intensity * 0.5f + 0.5f);
        Color c1 = new Color(.5f, 1f, 1f, 1f);
        Color c2 = new Color(1f, 1f, 1f, 1f);
        lit.color = Color.Lerp(c1,c2,intensity);
 

        //Scale object based on intensity
        float meshScale = Mathf.Max(initalMeshScale*0.5f, initalMeshScale * (intensity+1.2f));
        Vector3 meshScale3 = new Vector3(meshScale, meshScale, meshScale);
        gameObject.transform.localScale = meshScale3;

	}
}
