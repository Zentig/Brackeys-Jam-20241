using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperButton : MonoBehaviour
{
    public Collider Button;
    public Transform ButtonCheck;

    public EventHandler<bool> buttonPressed;

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
        buttonPressed?.Invoke(this, false);
    }

    public void Activate(){
        buttonPressed?.Invoke(this, true);
    }
}
