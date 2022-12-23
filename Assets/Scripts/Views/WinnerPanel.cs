using System;
using System.Collections;
using Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
   public class WinnerPanel : MonoBehaviour
   {
      public event Action OnExit; 

      [SerializeField, HideInInspector]
      private TMP_Text _labelTF;
      [SerializeField, HideInInspector]
      private Button _buttonExit;
      
      private void OnValidate()
      {
         _labelTF = GetComponentInChildren<TMP_Text>();
         _buttonExit = GetComponentInChildren<Button>();
      }

      public void Exit()
      {
         OnExit?.Invoke();
      }
      
      public void Show(Player player, int countdownTime)
      {
         gameObject.SetActive(true);
         _buttonExit.gameObject.SetActive(false);
         _labelTF.text = $"Player {player.Id} Win!";
         StartCoroutine(WaitCountdown(countdownTime));
      }

      private IEnumerator WaitCountdown(int countdownTime)
      {
         yield return new WaitForSeconds(countdownTime);
         _buttonExit.gameObject.SetActive(true);
      }
      
   }
}