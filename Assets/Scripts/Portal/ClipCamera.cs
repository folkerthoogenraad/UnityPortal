using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClipCamera : MonoBehaviour {

    public Shader clipShader;
    public Transform clippingPlane;
    
    void OnEnable()
    {
        GetComponent<Camera>().SetReplacementShader(clipShader, "");
    }
    
    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
    
    // Update is called once per frame
    void OnPreRender()
    {
        if (clippingPlane)
        {
            Shader.SetGlobalVector("ClippingPlanePosition", clippingPlane.position);
            Shader.SetGlobalVector("ClippingPlaneNormal", clippingPlane.up);
        }
    }
}
