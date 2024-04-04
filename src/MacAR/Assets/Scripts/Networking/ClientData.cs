//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
//Stores the data of a default user
[Serializable]
public class ClientData
{
    public ulong clientId;
    public int characterId = -1;

    public ClientData(ulong clientId)
    {
        this.clientId = clientId;
    }
}
