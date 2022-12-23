using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Controllerrs
{
   public class NetworkController : NetworkBehaviour
   {
      public event Action<ulong, string> OnClientSceneLoaded;
      public event Action<ulong> OnUserReadySRpc;
      
      [Inject] private NetworkManager _network;
      [Inject] private MainModel _model;
      
      public ulong ClientId => _network.LocalClient.ClientId;
      public bool IsListening => _network.IsListening;
      public NetworkSceneManager SceneManager => _network.SceneManager;
      public NetworkPrefabHandler PrefabHandler => _network.PrefabHandler;
      public bool ConnectedAsServer => IsServer;
      public bool ConnectedAsClient => IsClient;

      public double LocalTime => _network.LocalTime.Time;
      
      private void Start()
      {
         DontDestroyOnLoad(gameObject);
      }

      public void SetConnectionData(string address)
      {
         var transport = _network.NetworkConfig.NetworkTransport as UnityTransport;
         transport.SetConnectionData(CheckAddress(address), 7777);
      }
      
      public string CheckAddress(string dirtyString) => Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");

      public bool StartHost()
      {
         if (!_network.StartHost())
         {
            Debug.LogError("Can't start host");
            return false;
         }
         SignListeners();
         return true;
      }
      
      public void Disconnect()
      {
         _network.Shutdown();
      }
      
      public bool StarClient()
      {
         if (!_network.StartClient())
         {
            Debug.LogError("Can't start client");
            return false;
         }
         SignListeners();
         return true;
      }

      [ClientRpc]
      public void UpdateUsersStatesClientRpc(User[] players)
      {
         Debug.Log($"UpdatePlayersStatesClientRpc");
         _model.UpdateUsersStates(players);
      }
      
      [ClientRpc]
      public void UpdateUserStateClientRpc(User user)
      {
         Debug.Log($"UpdatePlayerStateClientRpc");
         _model.UpdateUserState(user.Id, user.CurState);
      }
      
      [ServerRpc(RequireOwnership = false)]
      public void UserReadyServerRpc(ulong clientId)
      {
         OnUserReadySRpc?.Invoke(clientId);      
      }
      
      private void SignListeners()
      {
         SceneManager.OnLoadComplete += ClientSceneLoaded;
         _network.OnClientDisconnectCallback += ClientDisconnected;
      }

      private void ClientDisconnected(ulong clientId)
      {
         Debug.Log($"ClientDisconnected clientId: {clientId};");
         _model.RemoveConnectedClient(clientId);
      }

      private void ClientSceneLoaded(ulong clientId, string sceneName, LoadSceneMode mode)
      {
         Debug.Log($"ClientSceneLoaded clientId:{clientId}; scene: {sceneName}");
         OnClientSceneLoaded?.Invoke(clientId, sceneName);
      }

      
   }
}