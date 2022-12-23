using System.Collections;
using Controllerrs;
using DefaultNamespace;
using Models;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Session.States
{
   public abstract class LobbyStateBase : GlobalSessionStateBase
   {
      
      [Inject] protected NetworkController _network;
      [Inject] protected MainModel _model;
      
      protected LobbyView _lobbyView;

      protected virtual void Init()
      {
         _model.UpdateUserState(_network.ClientId, User.State.EnteredToLobby);
         _lobbyView = FindObjectOfType<LobbyView>();
         _lobbyView.OnReady += OnReady;
      }

      protected virtual void OnReady()
      {
         _lobbyView.OnReady -= OnReady;
      }
   }
}