using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Physics.gravity = new Vector3(0, -9.81f, 0);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            Physics.gravity = new Vector3(-9.81f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            Physics.gravity = new Vector3(0, 0, -9.81f);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            Physics.gravity = new Vector3(0, 9.81f, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            Physics.gravity = new Vector3(9.81f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            Physics.gravity = new Vector3(0, 0, 9.81f);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.name == "Door")
            ChangeGravity(GravityDir.xP, other.transform);
    }

    public void ChangeGravity(GravityDir gravityDir, Transform ralativeTransform){
        switch(gravityDir){
            case GravityDir.yM:
                Physics.gravity = new Vector3(0, -9.81f, 0);
                break;
            case GravityDir.yP:
                Physics.gravity = new Vector3(0, 9.81f, 0);
                break;
            case GravityDir.xM:
                Physics.gravity = new Vector3(-9.81f, 0, 0);
                break;
            case GravityDir.xP:
                Physics.gravity = new Vector3(9.81f, 0, 0);
                break;
            case GravityDir.zM:
                Physics.gravity = new Vector3(0, 0, -9.81f);
                break;
            case GravityDir.zP:
                Physics.gravity = new Vector3(0, 0, 9.81f);
                break;
        }
        transform.rotation = ralativeTransform.rotation;
    }
}

[Serializable]
public enum GravityDir
{
    xP,
    xM,
    yP,
    yM,
    zP,
    zM
}
