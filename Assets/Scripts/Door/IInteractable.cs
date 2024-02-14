using System.Threading;
using Cinemachine;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void InteractWith(GameObject go, Camera camera, CancellationToken token);
    public abstract void Select();
    public abstract void Deselect();
    public abstract void TriggerEnter();
    public abstract void TriggerExit();
}
