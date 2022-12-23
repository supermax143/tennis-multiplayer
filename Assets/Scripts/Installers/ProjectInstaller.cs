using System;
using Controllerrs;
using DefaultNamespace;
using Models;
using Session;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Installers
{
   public class ProjectInstaller : MonoInstaller
   {

      [SerializeField, HideInInspector]
      private GlobalSession globalSession;
      [SerializeField, HideInInspector]
      private NetworkManager networkManager;
      [SerializeField, HideInInspector]
      private GameSettings settings;
      
      [SerializeField]
      private GameObject networkControllerGO;
      
      private void OnValidate()
      {
         globalSession = GetComponent<GlobalSession>();
         networkManager = GetComponent<NetworkManager>();
         settings = GetComponent<GameSettings>();
      }


      public override void InstallBindings()
      {
         Container.Bind<MainModel>().AsSingle();
         Container.BindInstance(globalSession).AsSingle();
         Container.BindInstance(settings).AsSingle();
         Container.BindInterfacesAndSelfTo<ScenesLoader>().AsSingle();
         Container.BindInstance(networkManager);
         var networkController = Instantiate(networkControllerGO).GetComponent<NetworkController>();
         Container.BindInstance(networkController).AsSingle();
         
      }
   }
}