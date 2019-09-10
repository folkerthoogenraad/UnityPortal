using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour {

    public bool mouseX = false;
    public bool mouseY = false;

    public float sensitivityX = 1;
    public float sensitivityY = -1;

    // Update is called once per frame
    void Update ()
    {
        if(mouseX)
            transform.localRotation *= Quaternion.Euler(new Vector3(0, Input.GetAxisRaw("Mouse X") * sensitivityX, 0));

        if(mouseY)
            transform.localRotation *= Quaternion.Euler(new Vector3(Input.GetAxisRaw("Mouse Y") * sensitivityY, 0, 0));
    }
}
