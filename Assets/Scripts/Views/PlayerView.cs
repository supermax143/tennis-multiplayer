using Models;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
   public class PlayerView : MonoBehaviour
   {

      [SerializeField]
      private TMP_Text nameTF;
      [SerializeField]
      private TMP_Text scoreTF;

      public void UpdateView(Player player)
      {
         nameTF.text = $"Player: {player.Id}";
         scoreTF.text = $"Score: {player.Score}";

      }
   }
}