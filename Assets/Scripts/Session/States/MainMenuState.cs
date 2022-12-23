using Controllerrs;
using DefaultNamespace;
using Models;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Session.States
{
   public class MainMenuState : GlobalSessionStateBase
   {

      [Inject] private NetworkController _network;

      private MainMenuView _mainMenu = null;
      
      protected override void OnStateEnter()
      {
         _mainMenu = FindObjectOfType<MainMenuView>();   
         _mainMenu.OnJoinGame += JoinGame;
         _mainMenu.OnCreateGame += CreateGame;
      }

      private void CreateGame()
      {
         _network.SetConnectionData(_mainMenu.GetAddress());
         if (!_network.StartHost())
         {
            return;
         }
         Unsign();
         _globalSession.SetState<LobbyStateHost>();
      }

      private void JoinGame()
      {
         _network.SetConnectionData(_mainMenu.GetAddress());
         if (!_network.StarClient())
         {
            return;
         }
         Unsign();
         _network.OnClientSceneLoaded += OnSceneLoaded;
      }

      private void Unsign()
      {
         _mainMenu.OnJoinGame -= JoinGame;
         _mainMenu.OnCreateGame -= CreateGame;
      }
      
      private void OnSceneLoaded(ulong clientId, string sceneName)
      {
         if (clientId != _network.ClientId && sceneName != SceneNames.LobbyScene)
         {
            return;
         }
         _network.OnClientSceneLoaded -= OnSceneLoaded;
         _globalSession.SetState<LobbyStateClient>();
      }
   }
}