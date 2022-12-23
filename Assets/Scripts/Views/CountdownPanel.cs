using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
   public class CountdownPanel : MonoBehaviour
   {
      [SerializeField, HideInInspector]
      private TMP_Text labelTF;

      private void OnValidate()
      {
         labelTF = GetComponentInChildren<TMP_Text>();
      }

      public void Show(int time)
      {
         StopAllCoroutines();
         labelTF.text = "";
         gameObject.SetActive(true);
         StartCoroutine(CountdownCoroutine(time));
      }

      private IEnumerator CountdownCoroutine(int time)
      {
         labelTF.text = time == 1 ? "Go" : time.ToString();
         yield return new WaitForSeconds(1);
         time--;
         if (time > 0)
         {
            StartCoroutine(CountdownCoroutine(time));
         }
         else
         {
            gameObject.SetActive(false);
         }
      }
      
   }
}