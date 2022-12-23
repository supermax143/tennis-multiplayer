using System;
using Session.States;
using TMPro;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
   public class MainMenuView : MonoBehaviour
   {

      [SerializeField, HideInInspector]
      private TMP_InputField inputTF = null;
      
      public Action OnCreateGame;
      public Action OnJoinGame;

      private void OnValidate()
      {
         inputTF = GetComponentInChildren<TMP_InputField>();
      }

      public string GetAddress() => inputTF.text;
      
      public void CreateGame() => OnCreateGame?.Invoke();

      public void JoinGame() => OnJoinGame?.Invoke();
   }
}