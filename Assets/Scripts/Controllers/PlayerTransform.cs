using System;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using Zenject;

namespace Controllerrs
{
   public class PlayerTransform : NetworkTransform
   {

      [Inject] private NetworkController _network;


      private bool CanCommit => _network != null && 
                                _network.ConnectedAsClient && 
                                _network.IsListening && 
                                IsOwner;

      protected override void Update()
      {
         base.Update();
         if (!CanCommit)
         {
            return;
         }
         TryCommitTransformToServer(transform, _network.LocalTime);
      }
      
      protected override bool OnIsServerAuthoritative()
      {
         return false;
      }
   }
}