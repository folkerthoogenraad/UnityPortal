using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PortalCamera : MonoBehaviour {
    
    [Header("Portal stuff")]
    public Transform cameraObject;

    public Transform viewPortal;
    public Transform destinationPortal;
    
    [Header("Shader stuff")]
    public Shader clipShader;

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
        Shader.SetGlobalVector("ClippingPlanePosition", destinationPortal.position);

        Shader.SetGlobalVector("ClippingPlaneNormal", destinationPortal.forward);
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        Vector3 localPos = viewPortal.InverseTransformPoint(cameraObject.position);

        Vector3 localUp = viewPortal.InverseTransformDirection(cameraObject.up);
        Vector3 localForward = viewPortal.InverseTransformDirection(cameraObject.forward);
        
        {
            localUp = Portal.DeflectY(localUp);
            localForward = Portal.DeflectY(localForward);
            localPos = Portal.DeflectY(localPos);
        }

        Vector3 newPos = destinationPortal.TransformPoint(localPos);

        Vector3 newUp = destinationPortal.TransformDirection(localUp);
        Vector3 newForward = destinationPortal.TransformDirection(localForward);
        
        transform.position = newPos;
        transform.rotation = Quaternion.LookRotation(newForward, newUp);
    }

    
}
