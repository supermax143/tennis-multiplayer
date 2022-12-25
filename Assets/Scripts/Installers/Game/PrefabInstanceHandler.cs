using System;
using Controllerrs;
using Game;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Installers
{
   public class PrefabInstanceHandler: MonoBehaviour, INetworkPrefabInstanceHandler
   {
      [SerializeField]
      private GameObject _prefab;

      [Inject] private DiContainer _container;
      [Inject] private GameField _gameField;
      
      public GameObject Prefab => _prefab;


      public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
      {
         var instance = Instantiate(Prefab);
         _container.InjectGameObject(instance);
         _gameField.SetPLayerPosition(instance.transform);
         return instance.GetComponent<NetworkObject>();
      }

      public void Destroy(NetworkObject networkObject)
      {
         Destroy(networkObject.gameObject);
      }
   }
}