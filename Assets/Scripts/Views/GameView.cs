using System;
using System.Collections.Generic;
using Controllerrs;
using Models;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
   public class GameView : NetworkBehaviour
   {
      public event Action OnExit; 

      [SerializeField]
      private PlayerView _firstPlayer;
      [SerializeField]
      private PlayerView _secondPlayer;
      [SerializeField]
      private CountdownPanel _countdown;
      [SerializeField]
      private WinnerPanel _winPanel;
      
      [ClientRpc]
      public void UpdatePlayersClientRpc(Player[] players)
      {
         _firstPlayer.UpdateView(players[0]);
         _secondPlayer.UpdateView(players[1]);
      }

      [ClientRpc]
      public void ShowWinnerClientRpc(Player player, int countdownTime)
      {
         _winPanel.OnExit += Exit;
         _winPanel.Show(player, countdownTime);
      }

      private void Exit()
      {
         _winPanel.OnExit -= Exit;
         OnExit?.Invoke();
      }

      [ClientRpc]
      public void ShowCountdownClientRpc(int countdownTime)
      {
         _countdown.Show(countdownTime);
      }
   }
}