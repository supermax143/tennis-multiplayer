using System.Collections;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Game.States
{
   public class StartNewRoundState : GameStateBase
   {
      private const int CountdownTime = 3;
      
      [Inject] private GameField _gameField;
      [Inject] private GameView _gameView;
      
      protected override void OnStateEnter()
      {
         _gameField.SpawnBall();
         _gameView.ShowCountdownClientRpc(CountdownTime);
         StartCoroutine(WaitStart(CountdownTime));
      }

      private IEnumerator WaitStart(int delay)
      {
         yield return new WaitForSeconds(delay);
         _gameController.SetState<ProcessGameState>();
      }
      
   }
}