using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllerrs;
using Models;
using Unity.Netcode;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using Zenject;

namespace Game.States
{
   public class GameController : NetworkBehaviour
   {
      public event Action OnGameFinished;

      
      [Inject] private NetworkController _network;
      [Inject] private MainModel _mainModel;
      [Inject] private DiContainer _container;
      [Inject] private GameSettings _settings;
      
      private List<Player> _players = new List<Player>(); 
      public IReadOnlyCollection<Player> Players => _players;
      
      public void StartGame(ulong firstClientId, ulong secondClientId)
      {
         if (!_network.ConnectedAsServer)
         {
            Debug.LogError("Only for Server");
            return;
         }

         if (!_mainModel.TryGetUser(firstClientId, out var firstUser))
         {
            Debug.LogError($"client with id:{ firstClientId} not found");
         }
         
         if (!_mainModel.TryGetUser(secondClientId, out var secondUser))
         {
            Debug.LogError($"client with id:{secondClientId} not found");
         }
         
         _players.Clear();
         
         _players.Add(new Player(firstUser));
         _players.Add(new Player(secondUser));
         
         SetState<InitGameState>();
      }
      
      private GameStateBase _curState;
      
      public T SetState<T>() where T : GameStateBase
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

      private Player GetOtherPlayer(Player player) => Players.First(p => p.Id != player.Id);

      public bool IsGameFinished => Players.Any(p => p.Score == _settings.MaxScoreCount);


      public void PlayerLose(Player loser)
      {
         var winner = GetOtherPlayer(loser);
         winner.Score++;
      }

      public Player GetWinner() => _players.FirstOrDefault(p => p.Score == _settings.MaxScoreCount);

      public void FinishGame()
      {
         OnGameFinished?.Invoke();
      }
   }
}