using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerPortalMove : MonoBehaviour {

    public Portal currentPortal = null;

    private PlayerMove motion;

    private bool beenBehind = false;

	// Use this for initialization
	void Start (){
        motion = GetComponent<PlayerMove>();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPortal)
        {
            Transform sourcePortal = currentPortal.transform;
            Transform destinationPortal = currentPortal.destination.transform;

            Vector3 localPosition = sourcePortal.InverseTransformPoint(transform.position);

            if (localPosition.z < 0)
                beenBehind = true;

            if(localPosition.z > 0 && beenBehind)
            {
                Vector3 localUp = sourcePortal.InverseTransformDirection(transform.up);
                Vector3 localForward = sourcePortal.InverseTransformDirection(transform.forward);
                Vector3 localVelocity = sourcePortal.InverseTransformVector(motion.GetVelocity());

                {
                    localUp = Portal.DeflectY(localUp);
                    localForward = Portal.DeflectY(localForward);
                    localPosition = Portal.DeflectY(localPosition);
                    localVelocity = Portal.DeflectY(localVelocity);
                }

                Vector3 newPos = destinationPortal.TransformPoint(localPosition);

                Vector3 newUp = destinationPortal.TransformDirection(localUp);
                Vector3 newForward = destinationPortal.TransformDirection(localForward);
                Vector3 newVelocity = destinationPortal.TransformVector(localVelocity);

                transform.position = newPos;
                transform.rotation = Quaternion.LookRotation(newForward, newUp);

                motion.SetVelocity(newVelocity);

                currentPortal = currentPortal.destination;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        currentPortal = other.transform.GetComponent<Portal>();
        beenBehind = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Portal t = other.transform.GetComponent<Portal>();
        if (currentPortal == t)
        {
            currentPortal = null;
        }
    }
}
