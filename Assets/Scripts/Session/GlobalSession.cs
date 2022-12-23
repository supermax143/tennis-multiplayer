using System;
using Session.States;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Session
{
   public class GlobalSession : MonoBehaviour
   {

      [Inject] private DiContainer _container;

      private GlobalSessionStateBase _curState;
      
      private void Start()
      {
         SetState<StartState>();
      }

      public T SetState<T>() where T : GlobalSessionStateBase
      {
         if (_curState != null && _curState.GetType() == typeof(T))
         {
            return _curState as T;
         }

         if (_curState != null)
         {
            _curState.ExitState();
         }
         _curState = _container.InstantiateComponent<T>(gameObject);
         return _curState as T;
      }
      
   }
}