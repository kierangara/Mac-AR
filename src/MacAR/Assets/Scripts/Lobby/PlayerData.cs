using System;
using UnityEngine;
using Unity.Netcode;

public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
{
    public ulong ClientId;
    public bool ReadyState;
    public Color CubeColor;

    public PlayerData(ulong clientId, bool readyState = false)
    {
        ClientId = clientId;
        ReadyState = readyState;
        CubeColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref ReadyState);
        serializer.SerializeValue(ref CubeColor);
    }

    public bool Equals(PlayerData other)
    {
        return ClientId == other.ClientId &&
            ReadyState == other.ReadyState;
    }
}