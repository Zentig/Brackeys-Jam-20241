using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using Player;
using Reflex.Attributes;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [field:SerializeField] public Door NextDoor { get; private set; }
    [field:SerializeField] public Transform CameraPivot { get; private set; }
    [field:SerializeField] public Transform SpawnPlayerPos { get; private set; }
    [field:SerializeField] public GravityDir GravitySwap { get; private set; }
    [SerializeField] private float _teleportTime;
    [SerializeField] private float _changeDutchTime = 0.4f;
    [SerializeField] private int _delayBeforeGetControl = 500;
    private Animator _animator;
    private static readonly int Opening = Animator.StringToHash("OpenState");

    private void Start() 
    {
        _animator = GetComponent<Animator>();
    }
    public override async void InteractWith(GameObject playerObject, Camera camera, CancellationToken token)
    {
        if (_cinemachineBrain != null) _cinemachineBrain.enabled = false;
        
        float _currentTeleportTime = 0;
        Vector3 startCameraPos = camera.transform.position;
        Quaternion startCameraRotation = camera.transform.rotation;
        
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

        if (playerMovement != null) {  
            playerMovement.SwitchWalkAbilityState(true);
        }

        while (_currentTeleportTime <= _teleportTime) 
        {
            camera.transform.SetPositionAndRotation(Vector3.Lerp(startCameraPos, NextDoor.CameraPivot.position, _currentTeleportTime / _teleportTime),
                                                    Quaternion.Lerp(startCameraRotation, NextDoor.CameraPivot.rotation, _currentTeleportTime / _teleportTime));
            _currentTeleportTime += Time.deltaTime;

            token.ThrowIfCancellationRequested();
            await Task.Yield();
        }
        await Task.Delay(_delayBeforeGetControl);
        token.ThrowIfCancellationRequested();

        playerMovement.ChangeCameraDutch(NextDoor.GravitySwap,_changeDutchTime);
        playerMovement.RotatePlayerByGravityDir(NextDoor.GravitySwap);

        var movePos = NextDoor.SpawnPlayerPos.transform.position;
        playerObject.transform.position = new Vector3(movePos.x,movePos.y,movePos.z);
        
        if (_cinemachineBrain != null) _cinemachineBrain.enabled = true;

        await Task.Delay(15);
        token.ThrowIfCancellationRequested();

        if (playerMovement != null) 
        { 
            playerMovement.ChangeGravity(NextDoor.GravitySwap/*, NextDoor.transform*/);    
            playerMovement.SwitchWalkAbilityState(false);
        } 
    }
    public override void Select()
    {
        _animator.SetBool(Opening, true);
    }
    public override void Deselect()
    {
        _animator.SetBool(Opening, false);
    }
    public override void TriggerEnter()
    {

    }
    public override void TriggerExit()
    {
        _animator.SetBool(Opening, false);
    }
}
