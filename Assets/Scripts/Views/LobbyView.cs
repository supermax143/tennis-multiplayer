using System;
using Controllerrs;
using Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
   public class LobbyView : MonoBehaviour
   {
      public event Action OnReady;

      [SerializeField]
      private TMP_Text _usersTF;

      
      [Inject] private MainModel _model;
      
      private void Start()
      {
         _model.OnUpdate += UpdateUsersList;
         UpdateUsersList();
      }

      private void UpdateUsersList()
      {
         _usersTF.text = "";
         string usersText = "";
         foreach (var user in _model.Users)
         {
            usersText += $"user: {user.Id} state: {user.CurState} \n";
         }

         _usersTF.text = usersText;
      }


      public void Ready() => OnReady?.Invoke();
   }
}