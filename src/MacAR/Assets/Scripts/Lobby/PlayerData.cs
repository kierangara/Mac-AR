//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
using Unity.Netcode;
//Stores the player's data in a structure that can be transmitted using the unity net code
[Serializable]
public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
{
    public ulong ClientId;
    public bool ReadyState;
    public PlayerData(ulong clientId, bool readyState = false)
    {
        ClientId = clientId;
        ReadyState = readyState;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref ReadyState);
    }

    public bool Equals(PlayerData other)
    {
        return ClientId == other.ClientId &&
            ReadyState == other.ReadyState;
    }
}