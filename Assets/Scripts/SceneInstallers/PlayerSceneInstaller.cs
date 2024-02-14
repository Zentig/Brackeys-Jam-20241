using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using Reflex.Core;
using UnityEngine;

public class PlayerSceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineBrain _cinemachineBrain;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.AddSingleton(_mainCamera);
        containerBuilder.AddSingleton(_cinemachineBrain);
    }
}
