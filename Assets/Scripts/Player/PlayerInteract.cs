using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField,Inject] private Camera _mainCamera;
    [SerializeField] private List<Interactable> _currentlyInteractedObjects;
    [SerializeField] private float _debounceTimer;
    private float _currentDebounceTime;

    private Interactable _lastInteractedObject;

    private void Update()
    {
        if (_currentDebounceTime > 0)
        {
            _currentDebounceTime -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        bool isInteractable = other.gameObject.TryGetComponent(out Interactable interactable);
        if (!isInteractable) return;

        if (interactable != null && !_currentlyInteractedObjects.Contains(interactable) && interactable != null) 
        {
            _currentlyInteractedObjects.Add(interactable);
        }

        SelectNextInteractable(interactable);
        interactable.TriggerEnter();
    }
    private void OnTriggerExit(Collider other)
    {
        bool isInteractable = other.gameObject.TryGetComponent(out Interactable interactable);
        if (!isInteractable) return;

        if (interactable != null && other.gameObject.activeSelf && _currentlyInteractedObjects.Contains(interactable))
        {
            _currentlyInteractedObjects.Remove(interactable);
            SelectNextInteractable(GetLastInteractable());
            interactable.TriggerExit();
        }
    }
    public void Interact(InputAction.CallbackContext ctx) 
    {
        if (_currentlyInteractedObjects.Count != 0 && _currentDebounceTime <= 0) 
        {
            _lastInteractedObject?.InteractWith(gameObject, _mainCamera, Application.exitCancellationToken);
            _currentlyInteractedObjects.Remove(_lastInteractedObject);
            SelectNextInteractable(GetLastInteractable());
            _currentDebounceTime = _debounceTimer;
        }
    }
    private Interactable GetLastInteractable() 
    {
        if ( _currentlyInteractedObjects.Count == 0 ) { return null; }
        else { return _currentlyInteractedObjects[^1]; }
    }
    private void SelectNextInteractable(Interactable interactable)
    {
        Unselect();
        if (interactable != null)
        {
            interactable.Select();
            _lastInteractedObject = interactable;
        }
    }
    void Unselect()
    {
        if (_lastInteractedObject != null)
        {
            _lastInteractedObject.Deselect();
            _lastInteractedObject = null;
        }
    }
}
