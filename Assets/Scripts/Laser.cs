using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    BoxCollider boxCollider;

    private LineRenderer lr;
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        lr.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }
        }
        else lr.SetPosition(1, transform.forward * 5000);

        boxCollider.center = new(0, 0, lr.GetPosition(1).z / 2);
        boxCollider.size = new(boxCollider.size.x, boxCollider.size.y, lr.GetPosition(1).z);
    }
}
