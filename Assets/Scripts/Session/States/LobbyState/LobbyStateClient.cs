using DefaultNamespace;
using Models;
using Session.States.GameState;
using UnityEngine;

namespace Session.States
{
  public class LobbyStateClient : LobbyStateBase
  {
    protected override void OnStateEnter()
    {
      Init();
    }

    protected override void OnReady()
    {
      base.OnReady();
      if (!_model.TryGetUser(_network.ClientId, out var user) || user.CurState == User.State.ReadyForGame)
      {
        return;
      }

      _network.OnClientSceneLoaded += OnSceneLoaded;
      _network.UserReadyServerRpc(user.Id);
    }
    
    private void OnSceneLoaded(ulong clientId, string sceneName)
    {
      _network.OnClientSceneLoaded -= OnSceneLoaded;
      if (clientId != _network.ClientId && sceneName != SceneNames.GameScene)
      {
        return;
      }
      _globalSession.SetState<InGameStateClient>();
    }
    
  }
}