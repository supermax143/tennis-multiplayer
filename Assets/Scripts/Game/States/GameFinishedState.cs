using System.Collections;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace Game.States
{
   public class GameFinishedState : GameStateBase
   {
      private const int CountdownTime = 3;
      
      [Inject] private GameView _gameView;
      protected override void OnStateEnter()
      {
         _gameView.ShowWinnerClientRpc(_gameController.GetWinner(), CountdownTime);
         StartCoroutine(WaitFinish(CountdownTime));
      }
      
      
      private IEnumerator WaitFinish(int delay)
      {
         yield return new WaitForSeconds(delay);
         _gameController.FinishGame();
      }
   }
}