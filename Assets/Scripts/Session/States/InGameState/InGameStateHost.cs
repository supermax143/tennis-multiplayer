using UnityEngine;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using Game.States;
using Models;
using Unity.VisualScripting;
using Zenject;

namespace Session.States.GameState
{
   public class InGameStateHost : InGameStateBase
   {
      
      [Inject] private ScenesLoader _scenesLoader;
      
      private bool IsCorrectScene => _scenesLoader.CurScene == SceneNames.GameScene;

      private User[] gameUsers;
      private GameController _gameController;
      
      protected override void OnStateEnter()
      {
         StartCoroutine(LoadGameScene());
      }

      private IEnumerator LoadGameScene()
      {
         _scenesLoader.LoadGameScene();

         yield return new WaitUntil(() => IsCorrectScene);
        
         Init();
      }

      protected override void Init()
      {
         gameUsers = _model.GetUsersWithState(User.State.ReadyForGame).ToArray();
         if (gameUsers.Length != 2)
         {
            Debug.LogError("Users count must be 2");
            return;
         }
         _gameController = FindObjectOfType<GameController>();
         base.Init();
         
         _network.OnClientSceneLoaded += OnClientLoadedScene;
         foreach (var user in gameUsers)
         {
            user.CurState = User.State.InGame;
         }
         _model.UpdateUsersStates(gameUsers);
      }

      private void OnClientLoadedScene(ulong clientId, string sceneName)
      {
         if (!gameUsers.Any(user => user.Id == clientId) || sceneName != SceneNames.GameScene)
         {
            return;
         }
         _network.OnClientSceneLoaded -= OnClientLoadedScene;
         _gameController.StartGame(_network.ClientId, clientId);
         _gameController.OnGameFinished += OnGameFinished;
      }

      private void OnGameFinished()
      {
         _gameController.OnGameFinished -= OnGameFinished;
         _network.Disconnect();
      }
   }
}