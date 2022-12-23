using System.Linq;
using DefaultNamespace;
using Models;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.States
{
   public class InitGameState : GameStateBase
   {
      [Inject] private GameField _gameField;
      [Inject] private GameController _gameController;
      [Inject] private GameView _gameView;
      
      protected override void OnStateEnter()
      {
         var players = _gameController.Players.ToArray();
         _gameField.InitPlayers(players);
         _gameView.UpdatePlayersClientRpc(players.ToArray());
         _gameController.SetState<StartNewRoundState>();
      }
      
   }
}