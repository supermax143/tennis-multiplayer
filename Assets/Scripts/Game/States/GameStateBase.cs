using Session;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.States
{
   public abstract class GameStateBase : NetworkBehaviour
   {
      [Inject] protected GameController _gameController;

      private void Start()
      {
         OnStateEnter();
      }

      public void ExitState()
      {
         Destroy(this);
      }

      protected abstract void OnStateEnter();
      
      protected virtual void OnStateExit() 
      {
      }

      private void OnDestroy()
      {
         OnStateExit();
      }
   }
}