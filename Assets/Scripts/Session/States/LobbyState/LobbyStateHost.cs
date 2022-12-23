using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Models;
using Session.States.GameState;
using UnityEngine;
using Zenject;

namespace Session.States
{
  public class LobbyStateHost : LobbyStateBase
  {
    [Inject] private ScenesLoader _scenesLoader;
    
    private bool IsCorrectScene => _scenesLoader.CurScene == SceneNames.LobbyScene;
    
    
    protected override void OnStateEnter()
    {
      StartCoroutine(LoadLobbyScene());
    }

    protected override void OnStateExit()
    {
      _network.OnClientSceneLoaded -= OnClientLoadedScene;
      _network.OnUserReadySRpc -= OnUserReady;
    }

    private IEnumerator LoadLobbyScene()
    {
      _scenesLoader.LoadLobbyScene();

      yield return new WaitUntil(() => IsCorrectScene);
        
      Init();
    }
    
    protected override void Init()
    {
      base.Init();
      _network.OnClientSceneLoaded += OnClientLoadedScene;
      _network.OnUserReadySRpc += OnUserReady;
    }

    private void OnUserReady(ulong clientId)
    {
      _model.UpdateUserState(clientId, User.State.ReadyForGame);
      _model.TryGetUser(clientId, out var player);
      _network.UpdateUserStateClientRpc(player);

      var readyUsers = _model.GetUsersWithState(User.State.ReadyForGame);
      if (readyUsers.Count() == 2)
      {
        _network.OnClientSceneLoaded -= OnClientLoadedScene;
        _network.OnUserReadySRpc -= OnUserReady;
        _globalSession.SetState<InGameStateHost>();
      }
      
    }

    private void OnClientLoadedScene(ulong clientId, string sceneName)
    {
      if (sceneName != SceneNames.LobbyScene)
      {
        return;
      }
      _model.UpdateUserState(clientId, User.State.EnteredToLobby);
      _network.UpdateUsersStatesClientRpc(_model.Users);
    }

    protected override void OnReady()
    {
      base.OnReady();
      OnUserReady(_network.ClientId);
    }
  }
}