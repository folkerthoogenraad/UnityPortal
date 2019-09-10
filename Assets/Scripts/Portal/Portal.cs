using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Portal destination;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    
    public static Vector3 DeflectY(Vector3 input)
    {
        // I don't fucking know how I call this. Transforms from portal local to portal local.
        return new Vector3(-input.x, input.y, -input.z);
    }
}
