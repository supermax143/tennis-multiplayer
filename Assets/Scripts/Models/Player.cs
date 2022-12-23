using Unity.Netcode;

namespace Models
{
   public class Player : INetworkSerializable
   {
      public ulong Id;
      public uint Score;

      public Player() { }
      public Player(User user)
      {
         Id = user.Id;
         Score = 0;
      }
      
      public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
      {
         serializer.SerializeValue(ref Id);
         serializer.SerializeValue(ref Score);
      }
      
   }
}