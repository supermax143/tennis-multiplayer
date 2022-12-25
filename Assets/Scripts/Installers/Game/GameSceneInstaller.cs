using System;
using Controllerrs;
using DefaultNamespace;
using Game;
using Game.States;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Installers
{
   public class GameSceneInstaller : MonoInstaller
   {
     
      [Inject] private DiContainer _container;
      [Inject] private NetworkController _network;

      [SerializeField]
      private GameController _gameController;
      [SerializeField]
      private GameField _gameField;
      [SerializeField]
      private GameView _gameView;
      
      [SerializeField, HideInInspector]
      private PrefabInstanceHandler[] _instanceHandlers;

      private void OnValidate()
      {
         _instanceHandlers = GetComponents<PrefabInstanceHandler>();
      }
      

      public override void InstallBindings()
      {
         _container.BindInstance(_gameController);
         _container.BindInstance(_gameField);
         _container.BindInstance(_gameView);
         foreach (var handler in _instanceHandlers)
         {
            BindInstanceHandler(handler);
         }
      }

      private void BindInstanceHandler(PrefabInstanceHandler handler)
      {
         _container.BindInstance(handler);
         _network.PrefabHandler.AddHandler(handler.Prefab, handler);
      }
   }
}