//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
using Unity.Netcode;
//Holds the lobby state of each user
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
