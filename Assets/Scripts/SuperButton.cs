using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperButton : MonoBehaviour
{
    public Collider Button;
    public Transform ButtonCheck;

    private void Update() {
        Collider[] hit = Physics.OverlapSphere(ButtonCheck.position, 0.1f);
        foreach (var hitCollider in hit)
        {
            if (hitCollider == Button){
                Activate();
            }
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
