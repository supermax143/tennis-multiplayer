using System;
using System.Collections;
using System.Collections.Generic;
using Controllerrs;
using Controllerrs.Bonus;
using Models;
using Unity.Netcode;
using UnityEngine;

public class BallActor : NetworkBehaviour
{
   [SerializeField, HideInInspector]
   private Rigidbody2D _rigidbody2D;
   [SerializeField, HideInInspector]
   private Collider2D _collider;

   public Rigidbody2D Rigidbody => _rigidbody2D;
   public Collider2D Collider => _collider;

   public Player CurPlayer { get; private set; }

   private void OnTriggerEnter2D(Collider2D collider)
   {
      var go = collider.gameObject;
      if (go.CompareTag("Bonus"))
      {
         HandleBonusCollision(go);
      }
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
      var go = collision.gameObject;
      
      if (go.CompareTag("Player"))
      {
         HandlePlayerCollision(go);
      }
      
   }

   private void HandlePlayerCollision(GameObject playerGo)
   {
      CurPlayer = playerGo.GetComponent<PlayerActor>().Player;
   }

   private void HandleBonusCollision(GameObject bonusGo)
   {
      if (CurPlayer == null)
      {
         return;
      }
      bonusGo.GetComponent<BonusView>().Collect(CurPlayer);
   }

   private void OnValidate()
   {
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _collider = GetComponent<Collider2D>();
   }

}
