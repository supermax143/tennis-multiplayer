using System.Collections;
using System.Linq;
using DefaultNamespace;
using Models;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.States
{
   public class ProcessGameState : GameStateBase
   {
      // private const float SpawnBonusDelay = 10;
      // private const float StartSpeed = 10;
      
      [Inject] private GameField _gameField;
      [Inject] private GameView _gameView;
      [Inject] private GameSettings _settings;
      
      protected override void OnStateEnter()
      {
         _gameField.OnPlayerLose += OnPlayerLose;
         _gameField.PushBall(_settings.BallStartSpeed);
         StartCoroutine(SpawnBonus(_settings.BonusSpawnDelay));
      }

      private void OnPlayerLose(Player player)
      {
         StopAllCoroutines();
         _gameField.OnPlayerLose -= OnPlayerLose;
         _gameField.DestroyBall();
         _gameController.PlayerLose(player);
         _gameView.UpdatePlayersClientRpc(_gameController.Players.ToArray());
         if (_gameController.IsGameFinished)
         {
            _gameController.SetState<GameFinishedState>();
         }
         else
         {
            _gameController.SetState<StartNewRoundState>();
         }
      }

      private IEnumerator SpawnBonus(float delay)
      {
         yield return new WaitForSeconds(delay);
         _gameField.SpawnBonus();
         StartCoroutine(SpawnBonus(_settings.BonusSpawnDelay));
      }
      
   }
}