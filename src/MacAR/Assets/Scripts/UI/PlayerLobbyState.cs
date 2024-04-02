using System;
using Unity.Netcode;
public struct PlayerLobbyState : INetworkSerializable, IEquatable<PlayerLobbyState>
{
    public ulong ClientId;
    public bool IsReady;

    public PlayerLobbyState(ulong clientId, bool isReady = false)
    {
        ClientId = clientId;
        IsReady = isReady;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref IsReady);
    }

    public bool Equals(PlayerLobbyState other)
    {
        return ClientId == other.ClientId &&
            IsReady == other.IsReady;
    }
}
