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

      [SerializeField]
      private GameController _gameController;
      [SerializeField]
      private GameField _gameField;
      [SerializeField]
      private GameView _gameView;
      

      public override void InstallBindings()
      {
         _container.BindInstance(_gameController);
         _container.BindInstance(_gameField);
         _container.BindInstance(_gameView);
      }
     
   }
}