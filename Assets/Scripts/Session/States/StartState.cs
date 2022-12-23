using System.Collections;
using DefaultNamespace;
using Models;
using UnityEngine;
using Zenject;

namespace Session.States
{
   public class StartState : GlobalSessionStateBase
   {
      [Inject] private ScenesLoader _scenesLoader;
      
      protected override void OnStateEnter()
      {
         if (_scenesLoader.CurScene != SceneNames.MainMenu)
         {
            _scenesLoader.LoadMainMenuScene();
         }

         StartCoroutine(CheckCurScene());
      }

      public IEnumerator CheckCurScene()
      {
         if (_scenesLoader.CurScene != SceneNames.MainMenu)
         {
            yield return null;
         }
         _globalSession.SetState<MainMenuState>();
      }
      
   }
}