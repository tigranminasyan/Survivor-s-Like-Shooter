using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerMovementController _playerMovementController;

    public override void InstallBindings()
    {
        Container.Bind<PlayerMovementController>().FromInstance(_playerMovementController).AsSingle();
    }
}

