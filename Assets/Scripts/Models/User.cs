using Unity.Netcode;

namespace Models
{
  public class User : INetworkSerializable
  {
    public enum State
    {
      EnteredToLobby,
      ReadyForGame,
      InGame
    }

    public ulong Id;
    public State CurState;

    public User() {}
    public User(ulong id)
    {
      Id = id;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Id);
      serializer.SerializeValue(ref CurState);
    }
  }
}