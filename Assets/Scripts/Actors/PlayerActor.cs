using System;
using Models;
using Unity.Netcode;
using UnityEngine;

namespace Controllerrs
{
   public class PlayerActor : NetworkBehaviour
   {
    
      private enum Direction
      {
         None, Left, Right
      }
      
      [SerializeField]
      private float _maxMoveSpeed = 10;
      [SerializeField]
      private float _acceleration = 1;
      [SerializeField]
      private bool _moveByMouse = true;
      
      [SerializeField, HideInInspector]
      private Rigidbody2D _rigidbody2D;
      [SerializeField, HideInInspector]
      private Collider2D _collider;
      
      private Direction _curDirection;
      private Camera _camera;
      
      public Player Player { get; private set; }
      
      private void OnValidate()
      {
         _rigidbody2D = GetComponent<Rigidbody2D>();
         _collider = GetComponent<Collider2D>();
      }


      private Camera GetCamera()
      {
         if (_camera == null)
         {
            _camera = Camera.main;
         }

         return _camera;
      }

      private Vector3 GetMouseWorldPosition() => GetCamera().ScreenToWorldPoint(Input.mousePosition);
      
      private void Update()
      {
         if (!IsOwner ||  !IsSpawned)
         {
            return;
         }

         if (_moveByMouse)
         {
            HandleMosePosition();
         }
         else
         {
            HandleKeyPress();
         }
         MoveByDirection();
      }

      public void HandleMosePosition()
      {
         var mousePosition = GetMouseWorldPosition();
         var distance = transform.position - mousePosition;
         var width = _collider.bounds.size.x;
         
         if (Math.Abs(distance.x) < width/2)
         {
            _curDirection = Direction.None;
            return;
         }

         _curDirection = distance.x < 0 ? Direction.Right : Direction.Left;
         
      }
      
      private void HandleKeyPress()
      {
         _curDirection = Direction.None;
         if (Input.GetKey(KeyCode.LeftArrow))
         {
            _curDirection = Direction.Left;
         }
         
         if(Input.GetKey(KeyCode.RightArrow))
         {
            _curDirection = Direction.Right;
         }

      }

      private void MoveByDirection()
      {

         var curAcceleration = _acceleration * Time.deltaTime;
         var curVelocity = _rigidbody2D.velocity.x;
         if (_curDirection == Direction.None)
         {
            if (Math.Abs(curVelocity) < curAcceleration)
            {
               _rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
               _rigidbody2D.velocity += new Vector2(curAcceleration * -Math.Sign(curVelocity),0);
            }
         }
         else
         {
            float dirSign = _curDirection is Direction.Left ? -1 : 1;
            _rigidbody2D.velocity += new Vector2(curAcceleration * dirSign,0);
         }
      }


      public void SetPlayer(Player player)
      {
         Player = player;
      }
   }
}