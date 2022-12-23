using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Android;
using Zenject;

namespace Session.States
{
   public abstract class GlobalSessionStateBase : MonoBehaviour
   {
      
      [Inject] protected GlobalSession _globalSession;

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