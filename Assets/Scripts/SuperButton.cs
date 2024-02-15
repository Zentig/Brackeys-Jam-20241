using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperButton : MonoBehaviour
{
    public Collider button;

    private void OnCollisionEnter(Collision other) {
        if(other.collider == button){
            Activate();
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.collider == button){
            Deactivate();
        }
    }

    private void Deactivate()
    {
        print("!SS");
    }

    public void Activate(){
        print("SS");
    }
}
