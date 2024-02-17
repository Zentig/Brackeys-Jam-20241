using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int Run = Animator.StringToHash("IsRunning");
    private readonly int Jump = Animator.StringToHash("Jump");
    
    [SerializeField] private PlayerMovement _playerMovement;
    private Animator _animator;

    private void Start() 
    {
        _animator = GetComponent<Animator>();
        _playerMovement.OnPlayerIdle += OnPlayerIdle;
        _playerMovement.OnPlayerRun += OnPlayerRun;
        _playerMovement.OnPlayerJump += OnPlayerJump;
    }

    private void OnPlayerRun(object sender, EventArgs e) 
    {
        _animator.SetBool(Run, true);
    }
    private void OnPlayerIdle(object sender, EventArgs e) 
    {
        _animator.SetBool(Run, false);
    }
    private void OnPlayerJump(object sender, EventArgs e) 
    {
        _animator.SetTrigger(Jump);
    }
}
