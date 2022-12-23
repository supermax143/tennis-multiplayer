using Controllerrs;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DefaultNamespace
{
   public class ScenesLoader  
   {
      [Inject] private ZenjectSceneLoader _sceneLoader;
      [Inject] private NetworkController _networkController;
      
      
      public string CurScene => SceneManager.GetActiveScene().name;


      public void LoadMainMenuScene() => LoadScene(SceneNames.MainMenu);
      public void LoadLobbyScene() => LoadScene(SceneNames.LobbyScene);
      public void LoadGameScene() => LoadScene(SceneNames.GameScene);
      
      private void LoadScene(string scene)
      {
         if (CurScene == scene)
         {
            return;
         }

         if (_networkController.IsListening)
         {
            _networkController.SceneManager.LoadScene(scene, LoadSceneMode.Single);
         }
         else
         {
            _sceneLoader.LoadScene(scene);
         }
      }
      

   }
}