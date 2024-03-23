using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting.FullSerializer;
[System.Serializable]
public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
{
    public ulong ClientId;
    public bool ReadyState;
// NativeString playerName;

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