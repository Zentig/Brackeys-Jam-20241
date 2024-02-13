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
}
