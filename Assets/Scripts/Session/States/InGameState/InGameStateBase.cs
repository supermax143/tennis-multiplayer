using Controllerrs;
using DefaultNamespace;
using Game;
using Models;
using UnityEngine;
using Zenject;

namespace Session.States.GameState
{
   public abstract class InGameStateBase : GlobalSessionStateBase
   {

      [Inject] protected NetworkController _network;
      [Inject] protected MainModel _model;
      
      protected GameField _gameField;
      protected GameView _gameView;
      
      protected virtual void Init()
      {
         _model.UpdateUserState(_network.ClientId, User.State.InGame);
         _gameField = FindObjectOfType<GameField>();
         _gameView = FindObjectOfType<GameView>();
         _gameView.OnExit += Exit;
      }

      private void Exit()
      {
         _model.Clear();
         _gameView.OnExit -= Exit;
         _network.Disconnect();
         _globalSession.SetState<StartState>();
      }
   }
}