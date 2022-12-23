using System;
using System.Collections.Generic;
using System.Linq;
using Controllerrs;
using Controllerrs.Bonus;
using Models;
using Unity.Netcode;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
   public class GameField : NetworkBehaviour
   {
      [Serializable]
      private struct PlayerInitData
      {
         [SerializeField]
         private Transform _playerSpawn;
         [SerializeField]
         private PlayerWall _playerWall;
         
         public Transform PlayerSpawn => _playerSpawn;
         public PlayerWall PlayerWall => _playerWall;
      }

      public event Action<Player> OnPlayerLose; 

      [SerializeField]
      private GameObject[] _bonusPrefabs;
      [SerializeField]
      private GameObject _playerPrefab;
      [SerializeField]
      private GameObject _ballPrefab;
      [SerializeField]
      private Transform _ballSpawn;
      [SerializeField]
      private PlayerInitData[] playersInitData;
      [SerializeField] 
      private BoxCollider2D _bonusesSpawn; 
      
      [Inject] private DiContainer _container;

      private BallActor _ball;
      private List<BonusView> _bonuses = new List<BonusView>();
      private List<PlayerActor> _players = new List<PlayerActor>();
      private List<BonusEffectBase> _effects = new List<BonusEffectBase>();
      public BallActor Ball => _ball;

      public bool TryGetPlayer(ulong id, out PlayerActor playerActor)
      {
         playerActor = _players.FirstOrDefault(p => p.Player.Id == id);
         return playerActor != null;
      }
      
      public bool TryGetOpponentPlayer(ulong id, out PlayerActor playerActor)
      {
         playerActor = _players.FirstOrDefault(p => p.Player.Id != id);
         return playerActor != null;
      }
      
      public void InitPlayers(IEnumerable<Player> players)
      {
         if (players.Count() != 2)
         {
            Debug.LogError("Players must be 2");
         }

         int index = 0;
         foreach (var player in players)
         {
            var data = playersInitData[index];
            data.PlayerWall.SetPlayer(player);
            data.PlayerWall.OnBallCollision += OnWallCollision;
            InstantiatePlayer(player, data.PlayerSpawn.position);
            index++;
         }
         
      }

      private void OnWallCollision(Player player)
      {
         OnPlayerLose?.Invoke(player);
      }

      public void SpawnBall()
      {
         if (!IsServer)
         {
            return;
         }
         
         var ballGo = Instantiate(_ballPrefab);
         _ball = ballGo.GetComponent<BallActor>();
         _container.InjectGameObject(ballGo);
         var instance = ballGo.GetComponent<NetworkObject>(); 
         instance.Spawn(true);
         instance.TrySetParent(transform);
         instance.transform.position = _ballSpawn.position;
      }

      public void SpawnBonus()
      {
         if (!IsServer)
         {
            return;
         }

         var bonusGO = Instantiate(_bonusPrefabs[Random.Range(0, _bonusPrefabs.Length)]);
         var bonus = bonusGO.GetComponent<BonusView>();
         bonus.OnCollect += OnBonusCollected;
         _bonuses.Add(bonus);
         _container.InjectGameObject(bonusGO);
         var instance = bonus.GetComponent<NetworkObject>(); 
         instance.Spawn(true);
         instance.TrySetParent(transform);
         var bounds = _bonusesSpawn.bounds;
         var pos = bounds.center;
         pos.x = Random.Range(bounds.min.x, bounds.max.x);
         instance.transform.position = pos;
      }

      private void OnBonusCollected(BonusView bonus)
      {
         var effect = BonusEffectBase.Create(bonus.EffectType, bonus.Player, bonus.EffectLifeTime, this);
         _effects.Add(effect);
         effect.OnDeactivated += OnEffectDeactivated;
         effect.Activate();
         bonus.OnCollect -= OnBonusCollected;
         _bonuses.Remove(bonus);
         bonus.GetComponent<NetworkObject>().Despawn();
      }

      private void OnEffectDeactivated(BonusEffectBase effect)
      {
         effect.OnDeactivated -= OnEffectDeactivated;
         _effects.Remove(effect);
      }

      private void InstantiatePlayer(Player player, Vector3 spawnPosition)
      {
         if (!IsServer)
         {
            return;
         }

         var playerGO = Instantiate(_playerPrefab);
         var playerActor = playerGO.GetComponent<PlayerActor>();
         playerActor.SetPlayer(player);
         _players.Add(playerActor);
         var instance = playerGO.GetComponent<NetworkObject>(); 
         instance.SpawnWithOwnership(player.Id, true);
         instance.TrySetParent(transform);
         instance.transform.position = spawnPosition;
      }

      public void PushBall(float startSpeed)
      {
         var ball = _ball;
         var dir = Random.insideUnitCircle.normalized;
         ball.Rigidbody.velocity = dir * startSpeed;
      }

      public void DestroyBall()
      {
         _ball.GetComponent<NetworkObject>().Despawn();
      }
   }
}